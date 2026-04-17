import React, { useEffect, useState } from 'react';
import { productService } from '../../../services/productService';
import { useNavigate } from 'react-router-dom';
import '../../../styles/components.css';

const ProductListPage = () => {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [suppliers, setSuppliers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [bulkCount, setBulkCount] = useState('');
    const [bulkLoading, setBulkLoading] = useState(false);

    const navigate = useNavigate();

    
    const [filters, setFilters] = useState({
        search: "",      
        categoryId: null, 
        pageNumber: 1,
        pageSize: 10
    });

    useEffect(() => {
        loadCategories();
        loadSuppliers();
    }, []);

    useEffect(() => {
        fetchProducts();
    }, [filters.pageNumber, filters.categoryId]); 

    const loadSuppliers = async () => {
        try {
            const res = await productService.getSuppliers();
            if (res.succeeded) setSuppliers(res.data);
        } catch (e) { console.error("Error en categorías", e); }
    };

    const loadCategories = async () => {
        try {
            const res = await productService.getCategories();
            if (res.succeeded) setCategories(res.data);
        } catch (e) { console.error("Error en categorías", e); }
    };

    const fetchProducts = async () => {
        setLoading(true);
        try {
            const cleanFilters = {
                ...filters,
                // Si search es un string vacío o solo espacios, enviamos null
                search: filters.search.trim() === null ? "" : filters.search,
                categoryId: filters.categoryId === null ? 0 : filters.categoryId
            };

            const result = await productService.getPaged(cleanFilters);
            if (result.succeeded) {
                setProducts(result.data);
            }
        } catch (err) {
            console.error("Error en búsqueda:", err);
        } finally {
            setLoading(false);
        }
    };

    const handleDeactivate = async (id) => {     
        const user = JSON.parse(localStorage.getItem('user') || '{}');

        const des = {
            productId: parseInt(id),
            updatedBy: parseInt(user.employeeId || 0)
        };

       
        if (window.confirm("¿Desea desactivar este producto?")) {
            try {
                const res = await productService.updateDesactivación(des);

                if (res.succeeded) {
                    fetchProducts();
                    alert("Producto desactivado correctamente");
                } else {
                    fetchProducts();
                    alert("Error: " + res.message);
                }
            } catch (error) {
                console.error("Error al desactivar:", error);
                alert("No se pudo conectar con el servidor");
            }
        }
    };

    const handleBulkInsert = async () => {
        // Validar que el campo no esté vacío
        if (!bulkCount || bulkCount.trim() === '') {
            alert("Por favor ingresa la cantidad de productos a generar");
            return;
        }

        const count = parseInt(bulkCount);
        if (isNaN(count) || count <= 0) {
            alert("Ingresa un número válido mayor a 0");
            return;
        }

       
        const user = JSON.parse(localStorage.getItem('user') || '{}');
        const createdBy = parseInt(user.employeeId || 0);

        if (createdBy === 0) {
            alert("No se pudo identificar el usuario. Por favor inicia sesión nuevamente");
            return;
        }

        setBulkLoading(true);
        try {
            const res = await productService.bulkInsertProducts({
                count: count,
                createdBy: createdBy
            });

            if (res.succeeded) {
                alert(`${count} productos generados exitosamente`);
                setBulkCount('');
                setFilters({ ...filters, pageNumber: 1 });
                fetchProducts();
            } else {
                alert("Error: " + (res.message || "No se pudieron generar los productos"));
            }
        } catch (error) {
            console.error("Error en carga masiva:", error);
            alert("No se pudo conectar con el servidor");
        } finally {
            setBulkLoading(false);
        }
    };

    return (
        <div className="product-list-page">
            <div className="product-list-page__header">
                <h2 className="product-list-page__title">Inventario de Autopartes</h2>
                <button
                    className="product-list-page__new-btn"
                    onClick={() => navigate('/products/new')}
                >
                    + Nuevo Producto
                </button>
            </div>
            <div className="product-list-page__filters">
                <input
                    className="product-list-page__search-input"
                    type="text"
                    placeholder="Buscar por nombre..."
                    value={filters.search}
                    onChange={(e) => setFilters({ ...filters, search: e.target.value })}
                />
                <select
                    className="product-list-page__category-select"
                    onChange={(e) => setFilters({ ...filters, categoryId: e.target.value ? parseInt(e.target.value) : null, pageNumber: 1 })}
                >
                    <option value="">Todas las Categorías</option>
                    {categories.map(c => (
                        <option key={c.categoryId} value={c.categoryId}>{c.categoryName}</option>
                    ))}
                </select>
                <button className="product-list-page__search-btn" onClick={() => { setFilters({ ...filters, pageNumber: 1 }); fetchProducts(); }}>
                    Buscar
                </button>
                <input
                    className="product-list-page__bulk-input"
                    type="number"
                    placeholder="Cantidad a generar..."
                    value={bulkCount}
                    onChange={(e) => setBulkCount(e.target.value)}
                    min="1"
                />
                <button 
                    className={bulkLoading ? "product-list-page__bulk-btn product-list-page__bulk-btn--loading" : "product-list-page__bulk-btn"}
                    onClick={handleBulkInsert}
                    disabled={bulkLoading}
                >
                    {bulkLoading ? 'Cargando...' : 'Cargar Masivamente'}
                </button>
            </div>
            {/* TABLA */}
            <table className="product-list-page__table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Nombre</th>
                        <th>Categoría</th>
                        <th>Precio</th>
                        <th>Stock</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    {loading ? (
                        <tr><td colSpan="6">Cargando datos...</td></tr>
                    ) : (
                        products.map(p => (
                            <tr key={p.productId}>
                                <td>{p.productId}</td>
                                <td>{p.productName}</td>
                                <td>{p.categoryName}</td>
                                <td>${p.unitPrice?.toFixed(3)}</td>
                                <td>{p.unitsInStock}</td>
                                <td>
                                    {/* Para editar, usaremos el objeto 'p' que ya tenemos en memoria */}
                                    <button className="product-list-page__edit-btn" onClick={() => {
                                        // 1. Buscamos el ID de la categoría basándonos en el nombre que sí tenemos en la tabla
                                        const foundCategory = categories.find(c => c.categoryName === p.categoryName);
                                        // 2. Buscamos el ID del proveedor basándonos en el nombre (si lo tienes en la tabla, si no, usa el que venga)
                                        const foundSupplier = suppliers.find(s => s.companyName === p.companyName);
                                        // 3. Creamos un objeto "reforzado" con los IDs encontrados
                                        const productWithIds = {
                                            ...p,
                                            categoryId: p.categoryId || foundCategory?.categoryId || foundCategory?.categoryid,
                                            supplierId: p.supplierId || foundSupplier?.supplierId || foundSupplier?.supplierid,
                                            quantityPerUnit: p.quantityPerUnit || p.QuantityPerUnit || p.quantityperunit || '' // Aseguramos que este campo también esté presente
                                        };
                                        console.log("Objeto enviado con IDs recuperados:", productWithIds);
                                        navigate(`/products/edit/${p.productId}`, {
                                            state: {
                                                product: productWithIds,
                                                categories,
                                                suppliers
                                            }
                                        });
                                    }}>
                                        Editar
                                    </button>
                                    <button
                                        className="product-list-page__deactivate-btn"
                                        onClick={() => handleDeactivate(p.productId)}
                                    >
                                        Desactivar
                                    </button>
                                </td>
                            </tr>
                        ))
                    )}
                </tbody>
            </table>
            {/* PAGINACIÓN */}
            <div className="product-list-page__pagination">
                <button className="product-list-page__pagination-btn" disabled={filters.pageNumber === 1} onClick={() => setFilters({ ...filters, pageNumber: filters.pageNumber - 1 })}>Anterior</button>
                <span className="product-list-page__page-info">Página {filters.pageNumber}</span>
                <button className="product-list-page__pagination-btn" disabled={products.length < filters.pageSize} onClick={() => setFilters({ ...filters, pageNumber: filters.pageNumber + 1 })}>Siguiente</button>
            </div>
        </div>
    );
};

export default ProductListPage;