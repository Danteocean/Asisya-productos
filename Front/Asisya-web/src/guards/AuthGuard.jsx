import { Navigate, Outlet } from 'react-router-dom';
import Header from '../components/Header';

const AuthGuard = () => {
    const token = localStorage.getItem('token');


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