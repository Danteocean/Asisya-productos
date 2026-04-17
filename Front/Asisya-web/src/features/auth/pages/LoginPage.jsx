import { useForm } from 'react-hook-form';
import { authService } from '../../../services/authService';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';

const LoginPage = () => {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [serverError, setServerError] = useState('');
  const navigate = useNavigate();


const onSubmit = async (data) => {
    try {
        const result = await authService.login(data.username, data.password);
        if (result.succeeded) {
            // ¡ESTA ES LA CLAVE!
            navigate('/products', { replace: true });
        } else {
            setServerError(result.message);
        }
    } catch (error) {
        setServerError("Error de conexión");
    }
};

  return (
    <div style={{ maxWidth: '400px', margin: '100px auto', padding: '20px', border: '1px solid #ccc' }}>
      <h2>Login Asisya</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label>Usuario</label>
          <input {...register("username", { required: "El usuario es obligatorio" })} />
          {errors.username && <p style={{color: 'red'}}>{errors.username.message}</p>}
        </div>
        
        <div style={{ marginTop: '10px' }}>
          <label>Contraseña</label>
          <input type="password" {...register("password", { required: "La clave es obligatoria" })} />
          {errors.password && <p style={{color: 'red'}}>{errors.password.message}</p>}
        </div>

        {serverError && <p style={{color: 'orange'}}>{serverError}</p>}

        <button type="submit" style={{ marginTop: '20px', width: '100%' }}>Entrar</button>
      </form>
    </div>
  );
};

export default LoginPage;