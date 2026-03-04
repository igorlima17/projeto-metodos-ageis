import { useNavigate } from 'react-router-dom';
import './register.css';

export default function PaginaCadastro() {
  const navegar = useNavigate();

  return (
    <div className="paginaRegister">
      <div className="caixaRegister">
        <div className="petshopRegister">
          <span className="logoRegister">Quatro Patas</span>
        </div>

        <div className="camposRegister">
          <label className="campoRegister">
            <span className="tituloCampoRegister">Nome</span>
            <input
              name="nome"
              type="text"
              placeholder="Seu nome completo"
              autoComplete="name"
            />
          </label>

          <label className="campoRegister">
            <span className="tituloCampoRegister">E-mail</span>
            <input
              name="email"
              type="email"
              placeholder="seu@email.com"
              autoComplete="email"
            />
          </label>

          <label className="campoRegister">
            <span className="tituloCampoRegister">Senha</span>
            <input
              name="senha"
              type="password"
              placeholder="••••••••"
              autoComplete="new-password"
            />
          </label>
        </div>

        <button className="botaoRegister">Criar conta</button>

        <p className="rodapeRegister">
          Já tem uma conta?{" "}
          <button className="linkRodapeRegister" onClick={() => navegar('/login')}>
            Entrar
          </button>
        </p>
      </div>
    </div>
  );
}