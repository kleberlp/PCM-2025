
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

var table = null;

$(document).ready(function () {

    Codebase.helpers(['datepicker', 'maxlength', 'select2']);

    carregarFiltro();
    carregarGrid();

    $('#unidade').on('change', function () {
        hasInventoryOpened();
    });

    $('#filtrar').click(function () {
        salvarFiltro();
        carregarGrid();
    });

    $('#closeInventory').click(function () {
        closeInventory();
    });

    if ($("#unidade").val() === "" || $("#unidade").val() === "-1") {
        $("#newInventory").hide();
        $("#closeInventory").hide();
    }

});

function carregarGrid() {

    var data = {
        unidade: ($('#unidade').val() == "") ? -1 : $('#unidade').val(),
        descricao: $('#descricao').val(),
        statusInventario: ($('#statusInventario').val() == "") ? -1 : $('#statusInventario').val(),
        dataInicio: $('#dataInicio').val(),
        dataTermino: $('#dataTermino').val()
    };

    loadGridMain({
        tableId: "#tbMain",
        data: data,
        endpoint: messages.urlLoadAssetInventoryMng,
        editAction: false,
        deleteAction: false,
        warningAction: false,
        customAction: true,
        enablePaging: true,
        pageLength: 15,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister,
        enableChild: true,
        customButtons: [
            {
                action: "managerInventory",
                icon: "fa fa-list",
                class: "btn btn-sm",
                title: messages.clickToManagerInventory,
                visible: (row) => row.status != "FINALIZADO",
                onClick: (row) => {
                    managerInventory(row);
                }
            }
        ],
        childRender: function (row) {
            return loadChildTable(row);
        }
    });

}

function managerInventory(data) {
    window.location = messages.urlAssetInventoryMngClose + '?codigo=' + data.codigoInventario;
}

function salvarFiltro() {

    var filtro = {
        unidade: $('#unidade').val(),
        descricao: $('#descricao').val(),
        dataInicio: $('#dataInicio').val(),
        dataTermino: $('#dataTermino').val(),
        statusInventario: $('#statusInventario').val()
    };

    localStorage.setItem("assetInventoryMngFiltro", JSON.stringify(filtro));
}

function carregarFiltro() {

    var filtro = JSON.parse(localStorage.getItem("assetInventoryMngFiltro") || "{}");

    if (!filtro) return;

    $('#unidade').val(filtro.unidade || "");
    $('#descricao').val(filtro.descricao || "");
    $('#dataInicio').val(filtro.dataInicio || "");
    $('#dataTermino').val(filtro.dataTermino || "");
    $('#statusInventario').val(filtro.statusInventario || "");
}

function hasInventoryOpened() {

    var unidade = $("#unidade").val();

    if (unidade === "" || unidade === "-1") {
        $("#newInventory").hide();
        $("#closeInventory").hide();
        return;
    }
    else
    {
        $.ajax({
            type: "POST",
            url: messages.urlHasInventoryOpened,
            async: false,
            data: {
                unidade: unidade
            },
            success: function (response) {
                if (response == 0) {
                    $("#newInventory").show();
                    $("#closeInventory").hide();
                } else {
                    $("#closeInventory").show();
                    $("#newInventory").hide();
                }
                
            }
        });
    }   
}

function loadChildTable(data) {

    var table_info = '';

    $.ajax({
        type: "POST",
        url: messages.urlLoadAssetInventoryMngDetails,
        async: false,
        data: {
            codigoInventario: data.codigoInventario
        },
        success: function (response) {

            table_info = `<div class="bg-light">
                <table class="table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th>${messages.asset}</th>
                            <th>${messages.descricao}</th>
                            <th>${messages.origem}</th>
                            <th>${messages.destino}</th>
                            <th class="text-center">${messages.usuario}</th>
                            <th class="text-center">${messages.data}</th>
                            <th class="text-center">${messages.ativoCadastrado}</th>
                        </tr>
                    </thead>
                    <tbody>`;

            response.forEach(r => {
                table_info += `
                    <tr>
                        <td class="text-center">${r.assetCode}</td>
                        <td>${r.descricao}</td>
                        <td>${r.origem}</td>
                        <td>${r.destino}</td>
                        <td class="text-center">${r.usuario}</td>
                        <td class="text-center">${r.data}</td>
                        <td class="text-center">${r.ativoCadastrado}</td>
                    </tr>`;
            });

            table_info += "</tbody></table></div>";
        }
    });

    return table_info;
}

async function closeInventory() {

    const confirmed = await rfConfirm({
        title: messages.msgQuestionCloseInventory,
        message: messages.msgNotPossibleReverse,
        confirmButtonText: messages.yes,
        cancelButtonText: messages.no
    });

    if (confirmed) {

        jQuery.ajax({
            method: "POST",
            url: messages.urlCloseInventory,
            async: true,
            data: {
                "unidade": $("#unidade").val()
            },
            dataType: "json",
            success: async function (response) {

                if (response.success) {

                    await rfAlert({
                        title: response.message,
                        message: "",
                        icon: "success",
                        confirmButtonText: messages.ok
                    });

                    carregarGrid();
                    hasInventoryOpened();

                } else {

                    await rfAlert({
                        title: response.message,
                        message: "",
                        icon: "error",
                        confirmButtonText: messages.ok
                    });
                }
            }
        });

    }

}