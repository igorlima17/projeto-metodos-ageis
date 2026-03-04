import { useNavigate } from 'react-router-dom';
import './login.css';

export default function PaginaLogin() {
  const navegar = useNavigate();

  return (
    <div className="paginaLogin">
      <div className="caixaLogin">
        <div className="petshopLogin">
          <span className="logoLogin">Quatro Patas</span>
        </div>

        <div className="camposLogin">
          <label className="campoLogin">
            <span className="tituloCampoLogin">E-mail</span>
            <input
              name="email"
              type="email"
              placeholder="seu@email.com"
              autoComplete="email"
            />
          </label>

          <label className="campoLogin">
            <span className="tituloCampoLogin">Senha</span>
            <input
              name="senha"
              type="password"
              placeholder="••••••••"
              autoComplete="current-password"
            />
          </label>
        </div>

        <button className="botaoLogin">Entrar</button>

        <p className="rodapeLogin">
          Ainda não tem conta?{" "}
          <button className="linkRodapeLogin" onClick={() => navegar('/register')}>
            Cadastre-se
          </button>
        </p>
      </div>
    </div>
  );
}