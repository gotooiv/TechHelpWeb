document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("formSolicitud");
    const apiUrl = "/api/solicitudes";
    const tablaBody = document.getElementById("tablaSolicitudes");
    const mensajeDiv = document.getElementById("mensajeGlobal");

    // Fecha actual por defecto
    document.getElementById("fechaSolicitud").value = new Date().toISOString().split("T")[0];

    // Cargar historial al inicio
    cargarSolicitudes();

    // Registrar nueva solicitud
    form.addEventListener("submit", async function (e) {
        e.preventDefault();
        const solicitud = {
            UsuarioId: usuarioId,
            NombreCompleto: document.getElementById("nombreCompleto").value.trim(),
            Correo: document.getElementById("correo").value.trim(),
            AreaSolicitanteId: parseInt(document.getElementById("areaId").value),
            TipoProblemaId: parseInt(document.getElementById("tipoId").value),
            PrioridadId: parseInt(document.getElementById("prioridadId").value),
            Descripcion: document.getElementById("descripcion").value.trim(),
            FechaSolicitud: document.getElementById("fechaSolicitud").value
        };
        if (!solicitud.AreaSolicitanteId || !solicitud.TipoProblemaId || !solicitud.PrioridadId) {
            mostrarMensaje("Complete todos los campos", "error");
            return;
        }
        if (solicitud.Descripcion.length < 10) {
            mostrarMensaje("Descripción mínimo 10 caracteres", "error");
            return;
        }
        try {
            const resp = await fetch(apiUrl, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(solicitud)
            });
            if (!resp.ok) throw new Error();
            mostrarMensaje("Solicitud registrada", "exito");
            form.reset();
            document.getElementById("fechaSolicitud").value = new Date().toISOString().split("T")[0];
            cargarSolicitudes();
        } catch (error) {
            mostrarMensaje("Error al registrar", "error");
        }
    });

    // Cargar todas las solicitudes y pintar tabla (solo botones Editar / Eliminar)
    async function cargarSolicitudes() {
        try {
            const resp = await fetch(apiUrl);
            if (!resp.ok) throw new Error("Error al cargar datos");
            const datos = await resp.json();
            if (!datos.length) {
                tablaBody.innerHTML = "<tr><td colspan='7'>No hay solicitudes registradas</td></tr>";
                return;
            }
            tablaBody.innerHTML = "";
            datos.forEach(s => {
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td>${s.Id}</td>
                    <td>${escapeHtml(s.NombreCompleto)}</td>
                    <td>${escapeHtml(s.AreaSolicitanteNombre)}</td>
                    <td>${escapeHtml(s.TipoProblemaNombre)}</td>
                    <td>${escapeHtml(s.PrioridadNombre)}</td>
                    <td><span class="badge ${claseEstado(s.EstadoNombre)}">${s.EstadoNombre}</span></td>
                    <td class="acciones">
                        <button class="btn-icon edit" onclick="editarSolicitud(${s.Id})" title="Editar">✏️</button>
                        <button class="btn-icon delete" onclick="eliminar(${s.Id})" title="Eliminar">🗑️</button>
                    </td>
                `;
                tablaBody.appendChild(row);
            });
        } catch (error) {
            console.error(error);
            tablaBody.innerHTML = "<tr><td colspan='7'>Error al cargar historial</td></tr>";
        }
    }

    // Cambiar estado (usado desde el modal)
    window.cambiarEstado = async (id, estadoId) => {
        try {
            const resp = await fetch(`${apiUrl}/${id}/estado`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(estadoId)
            });
            if (resp.ok) {
                mostrarMensaje("Estado actualizado", "exito");
                cargarSolicitudes();
                // Si el modal está abierto, actualizar el texto
                const modal = document.getElementById("modalEditar");
                if (modal.style.display === "flex") {
                    const estadoTexto = await obtenerNombreEstado(estadoId);
                    document.getElementById("estadoActualTexto").innerText = estadoTexto;
                    document.getElementById("editEstadoId").value = estadoId;
                }
                return true;
            } else {
                mostrarMensaje("Error al cambiar estado", "error");
                return false;
            }
        } catch (error) {
            mostrarMensaje("Error de red", "error");
            return false;
        }
    };

    async function obtenerNombreEstado(estadoId) {
        const estados = { 1: "Pendiente", 2: "En proceso", 3: "Resuelto" };
        return estados[estadoId] || "Desconocido";
    }

    // Eliminar solicitud
    window.eliminar = async (id) => {
        if (!confirm("¿Eliminar esta solicitud? No se puede deshacer.")) return;
        try {
            const resp = await fetch(`${apiUrl}/${id}`, { method: "DELETE" });
            if (resp.ok) {
                mostrarMensaje("Solicitud eliminada", "exito");
                cargarSolicitudes();
                cerrarModal();
            } else {
                mostrarMensaje("Error al eliminar", "error");
            }
        } catch (error) {
            mostrarMensaje("Error de red", "error");
        }
    };

    // Abrir modal de edición con datos actuales
    window.editarSolicitud = async (id) => {
        try {
            const resp = await fetch(`${apiUrl}/${id}`);
            if (!resp.ok) throw new Error();
            const s = await resp.json();
            document.getElementById("editId").value = s.Id;
            document.getElementById("editNombre").value = s.NombreCompleto;
            document.getElementById("editCorreo").value = s.Correo;
            document.getElementById("editAreaId").value = s.AreaSolicitanteId;
            document.getElementById("editTipoId").value = s.TipoProblemaId;
            document.getElementById("editPrioridadId").value = s.PrioridadId;
            document.getElementById("editDescripcion").value = s.Descripcion;
            document.getElementById("editFechaSolicitud").value = s.FechaSolicitud.split('T')[0];
            document.getElementById("editEstadoId").value = s.EstadoSolicitudId;
            document.getElementById("estadoActualTexto").innerText = s.EstadoNombre;
            document.getElementById("modalEditar").style.display = "flex";
        } catch (error) {
            mostrarMensaje("Error al cargar datos para editar", "error");
        }
    };

    // Cambiar estado desde el modal
    window.cambiarEstadoDesdeModal = async (nuevoEstadoId) => {
        const id = parseInt(document.getElementById("editId").value);
        if (!id) return;
        const ok = await cambiarEstado(id, nuevoEstadoId);
        if (ok) {
            const estadoTexto = await obtenerNombreEstado(nuevoEstadoId);
            document.getElementById("estadoActualTexto").innerText = estadoTexto;
            document.getElementById("editEstadoId").value = nuevoEstadoId;
        }
    };

    // Cerrar modal
    window.cerrarModal = function () {
        document.getElementById("modalEditar").style.display = "none";
    };

    // Guardar edición de campos (sin estado)
    window.guardarEdicion = async () => {
        const id = parseInt(document.getElementById("editId").value);
        const solicitudActualizada = {
            Id: id,
            UsuarioId: usuarioId,
            NombreCompleto: document.getElementById("editNombre").value.trim(),
            Correo: document.getElementById("editCorreo").value.trim(),
            AreaSolicitanteId: parseInt(document.getElementById("editAreaId").value),
            TipoProblemaId: parseInt(document.getElementById("editTipoId").value),
            PrioridadId: parseInt(document.getElementById("editPrioridadId").value),
            Descripcion: document.getElementById("editDescripcion").value.trim(),
            FechaSolicitud: document.getElementById("editFechaSolicitud").value
        };
        if (!solicitudActualizada.AreaSolicitanteId || !solicitudActualizada.TipoProblemaId || !solicitudActualizada.PrioridadId) {
            mostrarMensaje("Complete todos los campos", "error");
            return;
        }
        if (solicitudActualizada.Descripcion.length < 10) {
            mostrarMensaje("Descripción mínimo 10 caracteres", "error");
            return;
        }
        try {
            const resp = await fetch(`${apiUrl}/${id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(solicitudActualizada)
            });
            if (resp.ok) {
                mostrarMensaje("Solicitud actualizada", "exito");
                cerrarModal();
                cargarSolicitudes();
            } else {
                const error = await resp.json();
                mostrarMensaje("Error: " + JSON.stringify(error), "error");
            }
        } catch (error) {
            mostrarMensaje("Error de red", "error");
        }
    };

    function mostrarMensaje(texto, tipo) {
        mensajeDiv.textContent = texto;
        mensajeDiv.className = `message ${tipo}`;
        setTimeout(() => {
            mensajeDiv.textContent = "";
            mensajeDiv.className = "message";
        }, 4000);
    }

    function escapeHtml(str) {
        if (!str) return "";
        return str.replace(/[&<>]/g, function (m) {
            if (m === '&') return '&amp;';
            if (m === '<') return '&lt;';
            if (m === '>') return '&gt;';
            return m;
        });
    }

    function claseEstado(estado) {
        const map = { "Pendiente": "pendiente", "En proceso": "proceso", "Resuelto": "resuelto" };
        return map[estado] || "";
    }
});