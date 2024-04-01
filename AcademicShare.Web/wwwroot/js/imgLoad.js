$(document).ready(function () {
    $("#ChosseImg").change(function (e) {
        var url = $("#ChosseImg").val();
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (ChosseImg.files && ChooseImg.files[0] && (ext == "gif" || ext == "png" || ext == "jpg" || ext == "jpeg")) {
            var reader = new FileReader();
            reader.onload = function () {
                var output = document.getElementById('PrevImg');
                output.src = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
        }
    });
});