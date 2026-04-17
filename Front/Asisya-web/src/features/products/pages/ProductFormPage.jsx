import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { productService } from '../../../services/productService';

const ProductFormPage = () => {
  const { id } = useParams();
  const { state } = useLocation();
  const navigate = useNavigate();
  const isEdit = Boolean(id);

  const [categories, setCategories] = useState(state?.categories || []);
  const [suppliers, setSuppliers] = useState(state?.suppliers || []);
  const [loadingCategories, setLoadingCategories] = useState(true);
  const [loadingProduct, setLoadingProduct] = useState(isEdit);
  const [submitLoading, setSubmitLoading] = useState(false);
  const [loadError, setLoadError] = useState('');

  const { register, handleSubmit, reset, formState: { errors } } = useForm({
    defaultValues: {
      productName: '',
      categoryId: '',
      supplierId: '',
      quantityPerUnit: '',
      unitPrice: '',
      unitsInStock: '',
      reorderLevel: '',
      discontinued: 'false'
    }
  });

  useEffect(() => {
    const loadCatalogs = async () => {
      try {
        const [categoryRes, supplierRes] = await Promise.all([
          productService.getCategories(),
          productService.getSuppliers()
        ]);

        if (categoryRes?.succeeded) setCategories(categoryRes.data || []);
        if (supplierRes?.succeeded) setSuppliers(supplierRes.data || []);
      } catch (error) {
        console.error('Error cargando catálogos', error);
        setLoadError('No se pudieron cargar las categorías y proveedores.');
      } finally {
        setLoadingCategories(false);
      }
    };

    loadCatalogs();
  }, []);

  useEffect(() => {
    const loadProduct = async () => {
      if (!isEdit) {
        setLoadingProduct(false);
        return;
      }

      let productData = state?.product;

      if (!productData) {
        try {
          const res = await productService.getById(parseInt(id, 10));
          if (res?.succeeded && res.data) {
            productData = res.data;
          } else {
            setLoadError('No se pudo cargar el producto para editar.');
            setLoadingProduct(false);
            return;
          }
        } catch (error) {
          console.error('Error al obtener el producto', error);
          setLoadError('No se pudo cargar el producto para editar.');
          setLoadingProduct(false);
          return;
        }
      }

      reset({
        productName: productData.productName || productData.productname || '',
        categoryId: String(productData.categoryId || productData.CategoryId || productData.categoryid || productData.CategoryID || ''),
        supplierId: String(productData.supplierId || productData.SupplierId || productData.supplierid || productData.SupplierID || ''),
        quantityPerUnit: productData.quantityPerUnit || productData.QuantityPerUnit || productData.quantityperunit || '',
        unitPrice: productData.unitPrice != null ? String(productData.unitPrice) : '',
        unitsInStock: productData.unitsInStock != null ? String(productData.unitsInStock) : '',
        reorderLevel: productData.reorderLevel != null ? String(productData.reorderLevel) : '',
        discontinued: String(productData.discontinued ?? false)
      });

      setLoadError('');
      setLoadingProduct(false);
    };

    loadProduct();
  }, [id, isEdit, state?.product, reset]);

  const onSubmit = async (data) => {
    setSubmitLoading(true);
    try {
      const user = JSON.parse(localStorage.getItem('user') || '{}');
      const payload = {
        productId: isEdit ? parseInt(id, 10) : 0,
        productName: data.productName.trim(),
        categoryId: parseInt(data.categoryId, 10),
        supplierId: parseInt(data.supplierId, 10),
        quantityPerUnit: data.quantityPerUnit.trim(),
        unitPrice: parseFloat(data.unitPrice),
        unitsInStock: parseInt(data.unitsInStock, 10),
        reorderLevel: parseInt(data.reorderLevel, 10),
        discontinued: data.discontinued === 'true',
        ...(isEdit
          ? { updatedBy: user?.employeeId || 0 }
          : { createdBy: user?.employeeId || 0 })
      };

      const res = isEdit ? await productService.update(payload) : await productService.create(payload);
      if (res?.succeeded) {
        navigate('/products');
      } else {
        setLoadError(res?.message || 'No se pudo guardar el producto.');
      }
    } catch (error) {
      console.error('Error al guardar el producto', error);
      setLoadError('Error inesperado al guardar el producto.');
    } finally {
      setSubmitLoading(false);
    }
  };

  return (
    <div style={{ padding: '30px', maxWidth: '800px', margin: '0 auto' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '20px' }}>
        <h2>{isEdit ? "Editar Autoparte" : "Nueva Autoparte"}</h2>
        {/* BOTÓN PARA VOLVER */}
        <button type="button" onClick={() => navigate('/products')} style={{ padding: '8px 15px', cursor: 'pointer' }}>
          ← Volver a Productos
        </button>
      </div>

      <form onSubmit={handleSubmit(onSubmit)} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }}>
        <div style={{ gridColumn: 'span 2' }}>
          <label>Nombre</label>
          <input
            {...register('productName', { required: 'El nombre es obligatorio' })}
            style={{ width: '100%', padding: '8px' }}
          />
          {errors.productName && <span style={{ color: 'red' }}>{errors.productName.message}</span>}
        </div>

        <div>
          <label>Categoría</label>
          <select
            {...register('categoryId', { required: 'La categoría es obligatoria' })}
            style={{ width: '100%', padding: '8px' }}
          >
            <option value="">Seleccione...</option>
            {categories.map((c) => (
              <option key={c.categoryId} value={String(c.categoryId)}>{c.categoryName}</option>
            ))}
          </select>
          {errors.categoryId && <span style={{ color: 'red' }}>{errors.categoryId.message}</span>}
        </div>

        <div>
          <label>Proveedor</label>
          <select
            {...register('supplierId', { required: 'El proveedor es obligatorio' })}
            style={{ width: '100%', padding: '8px' }}
          >
            <option value="">Seleccione...</option>
            {suppliers.map((s) => (
              <option key={s.supplierId} value={String(s.supplierId)}>{s.companyName}</option>
            ))}
          </select>
          {errors.supplierId && <span style={{ color: 'red' }}>{errors.supplierId.message}</span>}
        </div>

        <div>
          <label>Cantidad por Unidad</label>
          <input
            {...register('quantityPerUnit', { required: 'La cantidad por unidad es obligatoria' })}
            style={{ width: '100%', padding: '8px' }}
          />
          {errors.quantityPerUnit && <span style={{ color: 'red' }}>{errors.quantityPerUnit.message}</span>}
        </div>

        <div>
          <label>Precio Unitario</label>
          <input
            type="number"
            step="0.0001"
            {...register('unitPrice', { required: 'El precio unitario es obligatorio' })}
            style={{ width: '100%', padding: '8px' }}
          />
          {errors.unitPrice && <span style={{ color: 'red' }}>{errors.unitPrice.message}</span>}
        </div>

        <div>
          <label>Stock</label>
          <input
            type="number"
            {...register('unitsInStock', { required: 'El stock es obligatorio' })}
            style={{ width: '100%', padding: '8px' }}
          />
          {errors.unitsInStock && <span style={{ color: 'red' }}>{errors.unitsInStock.message}</span>}
        </div>

        <div>
          <label>Nivel Reorden</label>
          <input
            type="number"
            {...register('reorderLevel', { required: 'El nivel de reorden es obligatorio' })}
            style={{ width: '100%', padding: '8px' }}
          />
          {errors.reorderLevel && <span style={{ color: 'red' }}>{errors.reorderLevel.message}</span>}
        </div>

        <div>
          <label>Estado</label>
          <select
            {...register('discontinued', { required: 'El estado es obligatorio' })}
            style={{ width: '100%', padding: '8px' }}
          >
            <option value="false">Activo</option>
            <option value="true">Discontinuado</option>
          </select>
          {errors.discontinued && <span style={{ color: 'red' }}>{errors.discontinued.message}</span>}
        </div>

        <button
          type="submit"
          disabled={submitLoading}
          style={{
            gridColumn: 'span 2',
            padding: '15px',
            backgroundColor: '#28a745',
            color: 'white',
            fontWeight: 'bold',
            cursor: submitLoading ? 'not-allowed' : 'pointer'
          }}
        >
          {submitLoading ? 'Guardando...' : isEdit ? 'ACTUALIZAR DATOS' : 'GUARDAR PRODUCTO'}
        </button>
      </form>
    </div>
  );
};

export default ProductFormPage;