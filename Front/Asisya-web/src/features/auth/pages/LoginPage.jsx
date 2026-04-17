import { useForm } from 'react-hook-form';
import { authService } from '../../../services/authService';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import '../../../styles/components.css';

const LoginPage = () => {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [serverError, setServerError] = useState('');
  const navigate = useNavigate();


const onSubmit = async (data) => {
    try {
        const result = await authService.login(data.username, data.password);
        if (result.succeeded) {
            navigate('/products', { replace: true });
        } else {
            setServerError(result.message);
        }
    } catch (error) {
        setServerError("Error de conexión");
    }
};

  return (
    <div className="login-page">
      <div className="login-page__container">
        <h2 className="login-page__title">Login Asisya</h2>
        <form className="login-page__form" onSubmit={handleSubmit(onSubmit)}>
          <div className="login-page__input-group">
            <label className="login-page__label">Usuario</label>
            <input className="login-page__input" {...register("username", { required: "El usuario es obligatorio" })} />
            {errors.username && <p className="login-page__error">{errors.username.message}</p>}
          </div>
          <div className="login-page__input-group">
            <label className="login-page__label">Contraseña</label>
            <input className="login-page__input" type="password" {...register("password", { required: "La clave es obligatoria" })} />
            {errors.password && <p className="login-page__error">{errors.password.message}</p>}
          </div>
          {serverError && <p className="login-page__server-error">{serverError}</p>}
          <button className="login-page__button" type="submit">Entrar</button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;