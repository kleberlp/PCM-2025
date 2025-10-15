
$(document).ready(function () {

    jQuery.validator.addMethod("email_bd", function (value, element) {

        var dataString = {
            "email": value,
            "codigo": 0
        };

        var result = false;

        jQuery.ajax({
            type: "POST",
            url: "Administracao/ValidaUsuario",
            async: false,
            data: dataString,
            dataType: "json",
            success: function (return_data) {
                if (return_data == false) {
                    result = false;
                } else if (return_data == true) {
                    result = true;
                }
            }
        });

        return result;

    }, "");
    
    $('#frmsidebar').validate({
        ignore: [],
        errorClass: 'invalid-feedback animated fadeInDown',
        errorElement: 'div',
        errorPlacement: function (error, e) {
            jQuery(e).parents('.form-group > div').append(error);
        },
        highlight: function (e) {
            jQuery(e).closest('.form-group').removeClass('is-invalid').addClass('is-invalid');
        },
        success: function (e) {
            jQuery(e).closest('.form-group').removeClass('is-invalid');
            jQuery(e).remove();
        },
        rules: {
            'email_perfil': {
                required: true,
                email: true,
                email_bd: true
            },
            'nome_perfil': {
                required: true
            },
            'senha_perfil': {
                required: true
            },
            'confirmar_senha_perfil': {
                required: true,
                equalTo: '#senha_perfil'
            }
        },
        messages: {
            'email_perfil': {
                required: 'Este campo é obrigatório.',
                email: 'E-mail inválido.',
                email_bd: 'Este E-mail já está associado a outro registro.'
            },
            'nome_perfil': {
                required: 'Este campo é obrigatório.'
            },
            'senha_perfil': {
                required: 'Este campo é obrigatório.',
            },
            'confirmar_senha_perfil': {
                required: 'Este campo é obrigatório.',
                equalTo: 'A Confirmação da Senha não corresponde com a Senha.',
            }
        },
        submitHandler: function (form) {
            var nome = document.getElementById("nome_perfil").value;
            var email = document.getElementById("email_perfil").value;
            var senha = document.getElementById("senha_perfil").value;

            $.post("Administracao/UsuarioUpdate",
            { "nome": nome, "email": email, "senha": senha },
            function (result) {
                //Informa o Usuário
                swal('Sucesso', 'Informações atualizadas com Sucesso!', 'success');
                document.getElementById("senha_perfil").value = "";
                document.getElementById("confirmar_senha_perfil").value = "";
                document.getElementById("btnClose").click();
            });
        }
    });
});
