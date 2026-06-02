const btnCamera = document.getElementById('btnCamera');
const btnGaleria = document.getElementById('btnGaleria');
const inCamera = document.getElementById('inputCamera');
const inGaleria = document.getElementById('inputGaleria');
const preview = document.getElementById('preview');
const fileInfo = document.getElementById('fileInfo');

// Abre câmera
btnCamera.addEventListener('click', () => {
    inCamera.value = ''; // reseta para permitir a mesma foto novamente
    if (inCamera.showPicker) inCamera.showPicker(); else inCamera.click();
});

// Abre galeria
btnGaleria.addEventListener('click', () => {
    inGaleria.value = ''; // reseta para permitir a mesma imagem novamente
    if (inGaleria.showPicker) inGaleria.showPicker(); else inGaleria.click();
});

// Mostra preview ao selecionar imagem
function mostrarPreview(input) {
    const file = input.files && input.files[0];
    if (!file) return;

    // Libera URL anterior para evitar leak de memória
    if (preview.src && preview.src.startsWith('blob:')) {
        URL.revokeObjectURL(preview.src);
    }

    const url = URL.createObjectURL(file);
    preview.src = url;
    preview.style.display = 'block';
    fileInfo.textContent = `${file.name} — ${(file.size / 1024 / 1024).toFixed(2)} MB`;
}

inCamera.addEventListener('change', () => mostrarPreview(inCamera));
inGaleria.addEventListener('change', () => mostrarPreview(inGaleria));

function goToPage(page, uniqueId) {
    window.location.href = page + "?uniqueId=" + uniqueId;
}