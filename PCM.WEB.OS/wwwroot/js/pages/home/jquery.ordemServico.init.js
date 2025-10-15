const btnCamera = document.getElementById('btnCamera');
const btnGaleria = document.getElementById('btnGaleria');
const inCamera = document.getElementById('inputCamera');
const inGaleria = document.getElementById('inputGaleria');
const preview = document.getElementById('preview');

// Abre câmera
btnCamera.addEventListener('click', () => {
    // alguns navegadores suportam showPicker(); se não, use click()
    if (inCamera.showPicker) inCamera.showPicker(); else inCamera.click();
});

// Abre galeria
btnGaleria.addEventListener('click', () => {
    if (inGaleria.showPicker) inGaleria.showPicker(); else inGaleria.click();
});

// Mostra preview ao selecionar
const onChange = (input) => {
    const file = input.files && input.files[0];
    if (!file) return;
    const url = URL.createObjectURL(file);
    preview.src = url;
    preview.style.display = 'block';
};

inCamera.addEventListener('change', () => onChange(inCamera));
inGaleria.addEventListener('change', () => onChange(inGaleria));

function goToPage(page, uniqueId) {

    window.location.href = page + "?uniqueId=" + uniqueId;
}
