//Guarda la petición fetch actual para poder cancelarla si el usuario escribe otra letra
let fetchActual = null;

// Se ejecuta al presionar el botón 🔍 de cliente
function abrirModalCliente() {
    // Obtiene el contenedor donde se inyectará la tabla de resultados
    const container = document.getElementById('resultadoClientes');
    // Muestra "Cargando..." mientras se espera la respuesta del servidor
    container.innerHTML = '<p class="text-muted text-center my-3">Cargando...</p>';
    // Crea una instancia del modal Bootstrap y lo muestra
    const modal = new bootstrap.Modal('#modalCliente');
    modal.show();
    // Dispara la primera búsqueda (sin filtro) para mostrar clientes disponibles
    buscarClientes();
}

// Busca clientes en el servidor según el término que escribió el usuario
function buscarClientes() {
    // Lee el valor del input de búsqueda (?.value por si el input aún no existe)
    const termino = document.getElementById('buscarClienteInput')?.value || '';
    // Construye la URL del endpoint que devuelve la Partial View con la tabla
    const url = '/Factura/BuscarClientes?termino=' + encodeURIComponent(termino);

    // Si hay una búsqueda anterior en curso, la cancela para evitar resultados obsoletos
    if (fetchActual) fetchActual.abort();
    fetchActual = new AbortController();

    // Hace la petición GET al servidor; el signal permite cancelarla si es necesario
    fetch(url, { signal: fetchActual.signal })
        // Convierte la respuesta en texto (HTML generado por la Partial View)
        .then(r => r.text())
        // Inyecta el HTML de la tabla dentro del contenedor del modal
        .then(html => {
            document.getElementById('resultadoClientes').innerHTML = html;
        })
        // Ignora el error de cancelación, muestra otros errores en la consola
        .catch(err => {
            if (err.name !== 'AbortError') console.error(err);
        });
}

// Escucha clicks en toda la página (event delegation) para capturar "Seleccionar" cliente
document.addEventListener('click', function (e) {
    // closest() busca hacia arriba el primer elemento con la clase .btn-seleccionar-cliente
    const btn = e.target.closest('.btn-seleccionar-cliente');
    if (btn) {
        // Lee el Id y nombre completo del cliente desde los atributos data-* del botón
        const id = btn.getAttribute('data-id');
        const nombre = btn.getAttribute('data-nombre');
        // Asigna el Id al hidden field del formulario (se envía al guardar la factura)
        document.getElementById('ClienteId').value = id;
        // Asigna el nombre al hidden y al display (el hidden asegura que sobreviva POSTs)
        document.getElementById('ClienteNombre').value = nombre;
        document.getElementById('ClienteNombreDisplay').value = nombre;
        // Cierra el modal de cliente
        bootstrap.Modal.getInstance('#modalCliente').hide();
    }
});

// ──────────── PRODUCTO ────────────

// Se ejecuta al presionar el botón 🔍 de producto (misma lógica que cliente)
function abrirModalProducto() {
    const container = document.getElementById('resultadoProductos');
    container.innerHTML = '<p class="text-muted text-center my-3">Cargando...</p>';
    const modal = new bootstrap.Modal('#modalProducto');
    modal.show();
    buscarProductos();
}

// Busca productos en el servidor según el término escrito
function buscarProductos() {
    const termino = document.getElementById('buscarProductoInput')?.value || '';
    const url = '/Factura/BuscarProductos?termino=' + encodeURIComponent(termino);

    if (fetchActual) fetchActual.abort();
    fetchActual = new AbortController();

    fetch(url, { signal: fetchActual.signal })
        .then(r => r.text())
        .then(html => {
            document.getElementById('resultadoProductos').innerHTML = html;
        })
        .catch(err => {
            if (err.name !== 'AbortError') console.error(err);
        });
}

// Escucha clicks en toda la página para capturar "Seleccionar" producto
document.addEventListener('click', function (e) {
    const btn = e.target.closest('.btn-seleccionar-producto');
    if (btn) {
        const id = btn.getAttribute('data-id');
        const nombre = btn.getAttribute('data-nombre');
        const descMax = parseInt(btn.getAttribute('data-descuento-maximo')) || 0;
        // Asigna el Id al hidden ProductoId (se envía al hacer clic en "Agregar")
        document.getElementById('ProductoId').value = id;
        // Actualiza el hidden ProductoNombre (viaja en el POST para preservar el nombre)
        document.getElementById('ProductoNombre').value = nombre;
        // Muestra el nombre en el input readonly del formulario
        document.getElementById('ProductoNombreDisplay').value = nombre;
        // Muestra el descuento maximo permitido como ayuda al usuario
        const info = document.getElementById('descuentoMaximoInfo');
        info.textContent = '⚠ Descuento máx: ' + descMax + '%';
        // Cierra el modal de producto
        bootstrap.Modal.getInstance('#modalProducto').hide();
    }
});