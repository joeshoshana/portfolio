$(document).ready(function () {
    $("nav").addClass("d-none");
    ClearAll();
    document.getElementsByTagName("body")[0]
        .addEventListener("keydown", function (event) {
            if (event.keyCode === 13) Enter();
        });
});

function Enter() {
    var obj = new U();

    if (obj.recaptchaResponse === "") {
        return alert("Please prove that you are not a robot");
    }

    $.ajax({
        type: "POST",
        url: "/Login/Enter",
        data: JSON.stringify({ data: JSON.stringify(obj) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                window.location.href = "Home/Index";
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Errorrrr");
            console.log(xhr.responseText);
        }
    });
}

function U() {
    this.Username = $("#username")
        .val()
        .trim();
    this.Password = $("#password").val();
    this.CompanyName = $("#company").val();
    //this.recaptchaResponse = grecaptcha.getResponse();
}
