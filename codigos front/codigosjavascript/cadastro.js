console.log("JS carregado");
const myForm = document.getElementById('cadastroCliente');

myForm.addEventListener('submit', function (event) {
    event.preventDefault();
    console.log("Botão clicado");

    const mensagemElemento = document.getElementById("Mensagem");

    // Rota corrigida com o prefixo /api/ exigido pelo seu Controller
    fetch('https://localhost:7174/Cliente', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            nome: document.getElementById("nome").value,
            email: document.getElementById("email").value,
            senha: document.getElementById("senha").value
        }),
    })
    .then(async response => {
        // Captura possíveis respostas de erro enviadas pelo servidor
        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || "Erro ao cadastrar cliente");
        }
        return response.json();
    })
    .then(data => {
        // Exibe o ID gerado que retornou do banco no formato correto do .NET (Created)
        mensagemElemento.innerHTML = `
            <h4 style='color:green'>
                Cliente cadastrado com sucesso! <br>
                Seu ID gerado foi: ${data.id}
            </h4>`;
            
        myForm.reset(); // Limpa os campos do formulário após o sucesso
    })
    .catch(error => {
        mensagemElemento.innerHTML = `
            <h4 style='color:red'>
                Erro: ${error.message}
            </h4>`;
            
        console.error("Detalhes do erro:", error);
    });
});
