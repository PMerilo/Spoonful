﻿function showImgPreview(input) {
    if (input.files[0]) {
        var uploadimg = new FileReader();
        uploadimg.onload = function (displayimg) {
            $("#imgPreview").attr('src', displayimg.target.result);
        }
        uploadimg.readAsDataURL(input.files[0]);
    }
}