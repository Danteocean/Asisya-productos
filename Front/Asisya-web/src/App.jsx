import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './features/auth/pages/LoginPage';
import ProductListPage from './features/products/pages/ProductListPage';
import ProductFormPage from './features/products/pages/ProductFormPage';
import AuthGuard from './guards/AuthGuard';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        <Route element={<AuthGuard />}>
          <Route path="/products" element={<ProductListPage />} />
          <Route path="/products/new" element={<ProductFormPage />} />
          <Route path="/products/edit/:id" element={<ProductFormPage />} />
          
        </Route>

        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;