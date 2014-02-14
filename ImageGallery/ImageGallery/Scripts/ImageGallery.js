var ImageGallery = {
    Init: function () {
        var images = document.getElementById('ImageListContainer').getElementsByTagName('img');
        for (var i = 0; i < images.length; i++) {
            //var origUri = images[i].getAttribute('src');
            //images[i].setAttribute('src', decodeURIComponent(origUri));
            //var newgUri = images[i].getAttribute('src');
            //console.log(origUri + ' => ' + newgUri);
        }
    }
};

window.onload = function () {
    ImageGallery.Init();
};