
//const labelsValues = {};
//let editMode = false;

//$(document).ready(function () {
//    document.getElementById('saveInputsBtn').style.display = 'none'
//    const alternativeInputs = document.getElementsByClassName('alternativeInput');
//    for (let i = 0; i < alternativeInputs.length; i++) {
//        alternativeInputs[i].style.display = "none";
//    }
    
//    const labels = document.getElementsByClassName('replaceable');
//    for (let i = 0; i < labels.length; i++) {
//        labelsValues[labels[i].id] = labels[i].innerText;
//    }

//})

//function showAltInputs() {
//    const inputs = document.getElementsByClassName('alternativeInput');
//    const labels = document.getElementsByClassName('replaceable');
//    if (editMode === false) {
//        for (let i = 0; i < inputs.length; i++) {
//            const labelId = inputs[i].id.replace('Alt', '');
//            inputs[i].value = labelsValues[labelId];
//            labels[i].style.display = "none";
//            inputs[i].style.display = "block";
//            document.getElementById('showInputsBtn').style.display = 'none'
//            document.getElementById('saveInputsBtn').style.display = 'block'
//        }
//        editMode = !editMode
//    }
//    else {
//        for (let i = 0; i < inputs.length; i++) {
//            inputs[i].style.display = "none";
//            labels[i].style.display = 'block';
//            document.getElementById('showInputsBtn').style.display = 'block'
//            document.getElementById('saveInputsBtn').style.display = 'none'
//        }
//        editMode = !editMode;
//    }
//}

//function saveChangedLabels() {
//    const inputs = document.getElementsByClassName('alternativeInput');
//    const inputsValues = {}
//    for (let i = 0; i < inputs.length; i++) {
//        const labelId = inputs[i].id.replace('Alt', '');
//        inputsValues[labelId] = inputs[i].value;
//        labelsValues[labelId] = inputsValues[labelId];
//        document.getElementById(labelId).innerText = labelsValues[labelId];
//    }
//    let company = {

//    }
//    $.ajax({
//        type: 'POST',
//        url: '/CompaniesSettings/Settings',
//        contentType: 'application/json; charset=utf-8',
//        data: JSON.stringify({ data: JSON.stringify(labelsValues) }),
//        dataType: 'json',
//        success: function (data) {

//            if (data.isSucceded == false) {
//                console.log(data.message);
//            }

//            else {
//                company = data.data;
//                console.log(data);
//                company.FormFields = labelsValues;
//                console.log(company);
//                console.log(JSON.stringify(company));
//                //$.ajax({
//                //    type: 'POST',
//                //    url: '/CompaniesSettings/Save',
//                //    contentType: 'application/json; charset=utf-8',
//                //    data: JSON.stringify(JSON.stringify(company)),
//                //    dataType: 'json',
//                //    success: function (data) {
//                //        if (data.isSucceded == false) {
//                //            console.log(data);
//                //        }
//                //        else {
//                //            console.log(data);
//                //        }
//                //    },
//                //    error: function (xhr, ajaxOptions, thrownError)
//                //    { console.log(xhr.responseText, thrownError) }
//                //});
//            }

//        },
//        error: function (xhr, ajaxOptions, thrownError)
//        { console.log(xhr.responseText) }
//    });
    
//    showAltInputs();
//}

// $.ajax({
//     type: 'GET',
//     url: '/VehiclesWeighing/SalesReport',
//     async: false,
//     contentType: 'application/json; charset=utf-8',
//     dataType: 'json',
//     success: function (data) {
//         if (data.isSucceded == false) {
//             alert(data);
//         }
//         else {

//             console.log(data);

//         }
//     },
//     error: function (xhr, ajaxOptions, thrownError)
//     { console.log(xhr.responseText) }

// });
