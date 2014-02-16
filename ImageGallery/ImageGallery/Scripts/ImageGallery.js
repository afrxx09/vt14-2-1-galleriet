var ImageGallery = {
    Init: function () {
        this.BindCloseButton();
    },

    BindCloseButton: function () {
        var closebutton = document.getElementById('closebutton');
        closebutton.addEventListener('click', function () {
            var dialog = document.getElementById('UploadSuccessContainer');
            dialog.style.display = 'none';
        });
    }
};

window.onload = function () {
    ImageGallery.Init();
};