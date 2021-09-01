var is_owner = false;
var is_super = false;
var company_tables_data = null;
var company_settings_data = null;
var company_forms_data = null;
var dictionary = null;
var units = null;
var readPermission = false;
var writePermission = false;

function Shared() {
    GetGreeting();
    Logo();
    GetCompanyTables();

    GetCompanyForms();
    GetCompanySettings();

    IsOwner();
    IsSuper();
    Dictionary();
    Units();
    toggle_item("dropdown_toggle");
    if (is_owner) $(".is_owner").show();
    else $(".is_owner").hide();

    if (is_super) $(".is_super").show();
    else {
        $(".is_super").hide();
    }
}

function ReadWritePermissions(data, id, type) {
    var obj = JSON.parse(data);
    var result = $.grep(obj, function(e) {
        return e.GUID == id;
    });
    if (result.length === 0) {
        console.log("No Items");
    } else {
        if (type == "w") return result[0].Write;
        else return result[0].Read;
    }
}

function ClearAll() {
    $.removeCookie("greeting");
    $.removeCookie("company_tables");
    $.removeCookie("company_forms");
}

function GetGreeting() {
    $.ajax({
        type: "POST",
        url: "/Home/Greeting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                $.cookie("greeting", data.message, { expires: 1, path: "/" });
                SetGreeting(data.message);
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function Units() {
    $.ajax({
        type: "POST",
        url: "/Home/Units",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                units = data.data;
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function Dictionary() {
    $.ajax({
        type: "POST",
        url: "/Home/Dictionary",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                dictionary = data.message;
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

const objectToCsv = function(data, columns,dates) {
    const csvRows = [];
    //get the headers
    const headers = [];
    

    if(dates !== undefined)
    {
        csvRows.push(dates.join(","));
    }

    columns.forEach(col => {
        headers.push(dictionary[col]);
    });
    csvRows.push(headers.join(","));

    //loop over the rows and clean the data
    for (const row of data) {
        const values = columns.map(col => {
            if (col == "InDate" || col == "OutDate") {
                row[col] = moment(row[col]).format("YYYY-MM-DD");
    }    
            const escaped = ("" + row[col]).replace(/"/g, '\\"');
            return row[col] == null ? "" : escaped;
        });
        csvRows.push(values.join(","));
    }
    return csvRows.join("\n");
};

function IsOwner() {
    $.ajax({
        async: false,
        type: "POST",
        url: "/Home/IsOwner",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                is_owner = data.data;
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function IsSuper() {
    $.ajax({
        async: false,
        type: "POST",
        url: "/Home/IsSuper",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                is_super = data.data;
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function SetGreeting(data) {
    $("#greeting").text(data);
}

function Disconnect() {
    alert("1");
}

function GetCompanyTables() {
    $.ajax({
        type: "POST",
        url: "/Home/CompanyTables",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                company_tables_data = JSON.stringify(data.data);
                SetCompanyTables(company_tables_data);
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function SetCompanyTables(data) {
    if (data !== undefined) {
        var dt = JSON.parse(data);
        var tables = $("#company_tables");
        $.each(dt, function(i, val) {
            tables.append(
                $('<a class="main_panel_item dropdown_item"/>')
                    .attr("href", val.Link)
                    .text(val.Name)
            );
        });
    }
}

function GetCompanyForms() {
    $.ajax({
        type: "POST",
        url: "/Home/CompanyForms",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                company_forms_data = JSON.stringify(data.data);
                SetCompanyForms(company_forms_data);
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function Logo() {
    $.ajax({
        type: "POST",
        url: "/Home/Logo",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                $(".system_logo").attr("src", data.data);
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function GetCompanySettings() {
    $.ajax({
        type: "POST",
        url: "/Home/CompanySettings",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                company_settings_data = JSON.stringify(data.data);
                SetCompanySettings(company_settings_data);
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function SetCompanyForms(data) {
    if (data !== undefined) {
        var dt = JSON.parse(data);
        var forms = $("#company_forms");
        $.each(dt, function(i, val) {
            forms.append(
                $('<a class="main_panel_item dropdown_item"/>')
                    .attr("href", val.Link)
                    .text(val.Name)
            );
        });
    }
}

function SetCompanySettings(data) {
    if (data !== undefined) {
        var dt = JSON.parse(data);
        var forms = $("#company_settings");
        $.each(dt, function(i, val) {
            forms.append(
                $('<a class="main_panel_item dropdown_item"/>')
                    .attr("href", val.Link)
                    .text(val.Name)
            );
        });
    }
}

function ToServerDate(jsDate, jsFormat) {
    var bd2 = moment(jsDate, jsFormat).locale("he");
    if (!bd2.isValid()) return null;
    var bd = bd2.valueOf() + bd2.utcOffset() * 60 * 1000;
    return "/Date(" + bd + ")/";
}

function InvisibleFields(formID) {
    var fields = "";
    $.ajax({
        type: "POST",
        url: "/Home/InvisibleFields",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ FormID: formID }),
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                fields = data.data;
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });

    return fields;
}

var groupBy = function(xs, key) {
    return xs.reduce(function(rv, x) {
        (rv[x[key]] = rv[x[key]] || []).push(x);
        return rv;
    }, {});
};