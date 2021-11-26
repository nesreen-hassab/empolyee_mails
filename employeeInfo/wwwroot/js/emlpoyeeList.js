var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/employee/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "name", "width": "20%" },
           {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                     <a href="/Demo/DemoViewAsPdf?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            send email
                        </a>
                        </div>`;
                }, "width": "70%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

