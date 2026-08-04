var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'streetAddress', "width": "15%"},
            { data: 'city', "width": "10%" },
            { data: 'state', "width": "20%" },
            { data: 'postalCode', "width": "15%" },
            {
                data: 'companyId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/company/upsert?companyId=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>
                                <a onClick=Delete("/admin/company/delete?companyId=${data}") class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i>Delete</a>
                            </div>`},
                    "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                       // toastr.success(data.message);

                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        //dataTable.ajax.reload();
                    }
                    else {
                        Swal.fire(
                            'Error!',
                            data.message,
                            'error'
                        );
                    }
                }
            });
        }
    });
}
