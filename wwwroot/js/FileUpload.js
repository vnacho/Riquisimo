
$(function () {
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                console.log($(input).attr("data-for"));
                console.log(e.target.result);
                $('img[data-for="' + $(input).attr("data-for") + '"').attr('src', e.target.result);
                $('#' + $(input).attr("data-for")).val(e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
        }
    }

    $(".file-upload").change(function () {
        readURL(this);
    });
});