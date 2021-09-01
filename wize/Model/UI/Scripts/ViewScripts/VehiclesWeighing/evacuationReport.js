const getEvacuationReport = async function() {
    try {
        const fromDateFilter = document.getElementById("fromDateInput").value;
        const toDateFilter = document.getElementById("toDateInput").value;

        const response = await fetch(
            "http://localhost:50587/VehiclesWeighing/EvacuationReport",
            {
                method: "GET",
                mode: "cors"
            }
        );

        const data = await response.json();
        const dataArray = data.data;

        const filteredByDateArray = filterByDate(
            dataArray,
            fromDateFilter,
            toDateFilter
        );

        const filteredDataArr = filteredByDateArray.map(dataRow => {
            const customerCode = dataRow.CustomerCode || "not Available";
            const customerName = dataRow.CustomerName || "not Available";
            const netto = dataRow.Netto || 0.000001;
            const guid = dataRow.GUID;

            return {
                customerCode,
                customerName,
                netto,
                guid
            };
        });

        const dataPivotByCustomerName = {};
        for (const record of filteredDataArr) {
            const { customerName, customerCode, netto, guid } = record; //Destructuring the record object
            if (dataPivotByCustomerName[customerName]) {
                continue;
            }
            dataPivotByCustomerName[customerName] = {
                customerCode,
                customerName,
                netto,
                appearances: 1
            };
            const currentCustomerName = customerName;
            const currentGuid = guid;
            for (const item of filteredDataArr) {
                const { customerName, netto, guid } = item; //Destructuring the record object
                if (
                    dataPivotByCustomerName[currentCustomerName]
                        .customerName === customerName &&
                    currentGuid !== guid
                ) {
                    dataPivotByCustomerName[customerName].appearances++;
                    dataPivotByCustomerName[customerName].netto += netto;
                }
            }
        }

        const dataForCsv = [];
        for (const item in dataPivotByCustomerName) {
            dataForCsv.push(dataPivotByCustomerName[item]);
        }
        const csvData = objectToCsv(dataForCsv);
        download(csvData);
    } catch (error) {
        console.log(error);
        alert("Something went wrong. please contact system administrator");
    }
};

const filterByDate = (objectsArray, fromDate, toDate) => {
    if (fromDate !== "") {
        if (toDate !== "") {
            const filteredArray = objectsArray.filter(row => {
                const inDate = moment(row.InDate).format("YYYY-MM-DD");
                return moment(inDate).isBetween(fromDate, toDate);
            });
            return filteredArray;
        }
        const filteredArray = objectsArray.filter(row => {
            const inDate = moment(row.InDate).format("YYYY-MM-DD");
            return moment(inDate).isAfter(fromDate);
        });
        return filteredArray;
    }

    if (toDate !== "") {
        const filteredArray = objectsArray.filter(row => {
            const inDate = moment(row.InDate).format("YYYY-MM-DD");
            return moment(inDate).isBefore(toDate);
        });
        return filteredArray;
    }
    return objectsArray;
};

// const objectToCsv = function(data, columns) {
//     const csvRows = [];
//     //get the headers
//     const headers = [];
//     columns.forEach(col => {
//         headers.push(dictionary[col]);
//     });
//     csvRows.push(headers.join(","));

//     //loop over the rows and clean the data
//     for (const row of data) {
//         const values = columns.map(col => {
//             const escaped = ("" + row[col]).replace(/"/g, '\\"');
//             return escaped;
//         });
//         csvRows.push(values.join(","));
//     }
//     return csvRows.join("\n");
// };

// const preview = function(data) {
//     if (!data.length) {
//         return `<div class="row text-center justify-content-center"><h6>No records</h6></div>`;
//     }
//     const headers = Object.keys(data[0]);
//     let titlesView = `<th scope="col">#</th>`;
//     headers.forEach(title => {
//         titlesView += `<th scope="col">${title}</th>`;
//     });

//     let dataView = ``;
//     let rowCounter = 0;
//     data.forEach(row => {
//         rowCounter++;
//         dataView += `<tr><th scope="row">${rowCounter}</th>`;
//         headers.forEach(header => {
//             dataView += `<td>${row[header]}</td>`;
//         });
//         dataView += `</tr>`;
//     });
//     const tableView = `
//         <table class="table">
//             <thead>
//                 <tr>
//                     ${titlesView}
//                 </tr>
//             </thead>
//             <tbody>
//                 ${dataView}
//             </tbody>
//         </table>
//     `;

//     return tableView;
// };

const download = function(csvData) {
    const blob = new Blob(["\ufeff" + csvData], {
        type: 'type: "text/csv;charset=UTF-8"'
    });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.setAttribute("hidden", "");
    a.setAttribute("href", url);
    a.setAttribute("download", "download.csv");
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
};
