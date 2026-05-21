document.getElementById("loginForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const email = document.getElementById("loginEmail").value;
    const senhaRaw = document.getElementById("loginSenha").value;
    
    // Converte para número inteiro puro
const senha = parseInt(document.getElementById("loginSenha").value);


    // Diagnóstico no console: Verifique se a senha aparece em AZUL (número) e não PRETO/VERDE (texto)
    const dadosDoEnvio = { email: email, senha: senha, nome: " " };
    console.log("Dados enviados para o SQL Server:", dadosDoEnvio);

    if (isNaN(senha)) {
        alert("Por favor, digite apenas números na senha!");
        return;
    }

    try {
        const response = await fetch('https://localhost:7174/Cliente/login', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dadosDoEnvio)
        });

        if (!response.ok) {
            if (response.status === 401) {
                throw new Error("E-mail ou senha não encontrados no banco de dados.");
            }
            throw new Error("Erro interno no servidor.");
        }

        // Se o SQL Server encontrar, ele retorna o ID, Nome e E-mail aqui:
        const usuario = await response.json();
        console.log("Usuário encontrado no SQL Server:", usuario);

        // Salva as credenciais para usar nas próximas telas
        sessionStorage.setItem("clienteId", usuario.id);
        sessionStorage.setItem("clienteNome", usuario.nome);

        alert(`Bem-vindo(a), ${usuario.nome}!`);
        window.location.href = "tarefas.html";

    } catch (error) {
        console.error(error);
        alert(error.message);
    }
});
