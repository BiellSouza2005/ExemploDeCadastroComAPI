const apiUrl = 'http://localhost:5046/AdicionarCliente';


function Salvar() {
    var nome = document.getElementById('nome').value;
    var cpf = document.getElementById('cpf').value;
    var usuario = document.getElementById('usuario').value;
    var senha = document.getElementById('senha').value;
    var dataNascimento = document.getElementById('dataNascimento').value;
    var cidade = document.getElementById('cidade').value;
    var estado = document.getElementById('estado').value;

    var Form = document.getElementById('FormID');
    if (Form.checkValidity() == false) 
    {
        var list = Form.querySelectorAll(':invalid');
        for (var item of list) {
            item.focus();
        }
    }
    else {
        var cliente = 
        {
            Id: 0,
            Nome: nome,
            CPF: cpf,
            Usuario: usuario,
            Senha: senha, 
            DataNascimento: dataNascimento,
            Cidade: cidade,
            Estado: estado
        };

        if(SalvarCliente(cliente))
        {
            alert("Cadastro realizado com sucesso!");
        }
        else 
        {
            alert("Ocorreu um erro ao realizar o cadastro, verifique o log para mais detalhes.");
        }
    }


    
}

function SalvarCliente(cliente){

    try {
        // Make a GET request
        fetch(apiUrl, {
            method: "POST",
            body: JSON.stringify(cliente),
            headers: 
            {
            "Content-type": "application/json; charset=UTF-8"
            }
        }); 
        return true;
    } 
    catch (error) 
    {
        console.log(cliente);
        console.log(error);
        return false;
    }
    

}




document.getElementById('cpf').addEventListener('input', function (e) {
    let value = e.target.value.replace(/\D/g, ''); // Remove todos os caracteres não numéricos
    value = value.replace(/^(\d{3})(\d)/, '$1.$2'); // Adiciona o primeiro ponto
    value = value.replace(/(\d{3})(\d)/, '$1.$2'); // Adiciona o segundo ponto
    value = value.replace(/(\d{3})(\d{1,2})$/, '$1-$2'); // Adiciona o traço
    e.target.value = value;
});