import React, { useEffect, useState } from 'react';
import { authService } from '../../src/services/authService';
import '../styles/components.css';

const Header = () => {
  const [userName, setUserName] = useState('');

  useEffect(() => {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    setUserName(user.name || user.username || 'Usuario');
  }, []);

  const handleLogout = () => {
    authService.logout();
  };

  return (
    <header className="header">
      <h3 className="header__title">Asisya - Inventario</h3>
      <div className="header__user-section">
        <span className="header__welcome-text">Bienvenido, {userName}</span>
        <button 
          className="header__logout-btn"
          onClick={handleLogout}
        >
          Cerrar Sesión
        </button>
      </div>
    </header>
  );
};

export default Header;