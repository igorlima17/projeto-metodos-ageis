import { BrowserRouter, Routes, Route } from 'react-router-dom';
import LoginPage from './pages/login/login';
import RegisterPage from './pages/register/register';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
      </Routes>
    </BrowserRouter>
  );
}