import React, { useEffect, useState } from 'react';
import { productService } from '../../../services/productService';
import { useNavigate } from 'react-router-dom';

const ProductListPage = () => {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [suppliers, setSuppliers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [bulkCount, setBulkCount] = useState('');
    const [bulkLoading, setBulkLoading] = useState(false);

    const navigate = useNavigate();

    // Estado alineado con tu DTO de búsqueda
    const [filters, setFilters] = useState({
        search: "",      // Texto para el tsvector
        categoryId: null, // Filtro por categoría
        pageNumber: 1,
        pageSize: 10
    });

    useEffect(() => {
        loadCategories();
        loadSuppliers();
    }, []);

    useEffect(() => {
        fetchProducts();
    }, [filters.pageNumber, filters.categoryId]); // Se dispara al cambiar página o combo

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
            // Limpiamos los filtros antes de enviar
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
        // 1. Extraemos solo el ID del empleado (ej: employeeId) para que el API lo reciba bien
        const user = JSON.parse(localStorage.getItem('user') || '{}');

        const des = {
            productId: parseInt(id),
            updatedBy: parseInt(user.employeeId || 0)
        };

        // 2. El bloque IF debe envolver la llamada al servicio
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

        // Obtener el usuario desde localStorage
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
        <div style={{ padding: '20px' }}>
            <h2>Inventario de Autopartes</h2>

            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                <h2>Inventario de Autopartes</h2>

                <button
                    onClick={() => navigate('/products/new')}
                    style={{ backgroundColor: '#007bff', color: 'white', padding: '10px 20px', borderRadius: '5px', cursor: 'pointer' }}
                >
                    + Nuevo Producto
                </button>
            </div>

            {/* FILTROS Y BUSQUEDA */}
            <div style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>
                <input
                    type="text"
                    placeholder="Buscar por nombre..."
                    value={filters.search}
                    onChange={(e) => setFilters({ ...filters, search: e.target.value })}
                />

                <select
                    onChange={(e) => setFilters({ ...filters, categoryId: e.target.value ? parseInt(e.target.value) : null, pageNumber: 1 })}
                >
                    <option value="">Todas las Categorías</option>
                    {categories.map(c => (
                        <option key={c.categoryId} value={c.categoryId}>{c.categoryName}</option>
                    ))}
                </select>

                <button onClick={() => { setFilters({ ...filters, pageNumber: 1 }); fetchProducts(); }}>
                    Buscar
                </button>

                <input
                    type="number"
                    placeholder="Cantidad a generar..."
                    value={bulkCount}
                    onChange={(e) => setBulkCount(e.target.value)}
                    min="1"
                    style={{ width: '150px' }}
                />

                <button 
                    onClick={handleBulkInsert}
                    disabled={bulkLoading}
                    style={{ backgroundColor: '#28a745', color: 'white', padding: '10px 20px', borderRadius: '5px', cursor: bulkLoading ? 'not-allowed' : 'pointer', opacity: bulkLoading ? 0.6 : 1 }}
                >
                    {bulkLoading ? 'Cargando...' : 'Cargar Masivamente'}
                </button>
            </div>

            {/* TABLA */}
            <table border="1" style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead style={{ backgroundColor: '#eee' }}>
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
                                    <button onClick={() => {
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
                                        onClick={() => handleDeactivate(p.productId)}
                                        style={{ marginLeft: '10px', color: 'red' }}
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
            <div style={{ marginTop: '10px' }}>
                <button disabled={filters.pageNumber === 1} onClick={() => setFilters({ ...filters, pageNumber: filters.pageNumber - 1 })}>Anterior</button>
                <span style={{ margin: '0 10px' }}>Página {filters.pageNumber}</span>
                <button disabled={products.length < filters.pageSize} onClick={() => setFilters({ ...filters, pageNumber: filters.pageNumber + 1 })}>Siguiente</button>
            </div>
        </div>
    );
};

export default ProductListPage;