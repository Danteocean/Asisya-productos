import { Navigate, Outlet } from 'react-router-dom';
import Header from '../components/Header';

const AuthGuard = () => {
    const token = localStorage.getItem('token');

    // Si hay token, muestra header y contenido, si no, al login
    return token ? (
        <>
            <Header />
            <Outlet />
        </>
    ) : (
        <Navigate to="/login" replace />
    );
};

export default AuthGuard;