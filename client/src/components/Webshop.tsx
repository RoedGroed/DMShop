import '../ProductList.css';
import ProductList from './webshop/ProductsList.tsx'
import React, {useEffect, useState} from "react";
import { http } from "../http";
import {ProductDto, PropertyDto} from "../Api.ts";
import {useCart} from "./webshop/CartContext.tsx";



const Webshop: React.FC = () => {
    const [products, setProducts] = useState<ProductDto[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const {addToCart} = useCart();
    const [selectedProduct, setSelectedProduct] = useState<number[]>([]);
    const [properties, setProperties] = useState<{ id: number, name: string }[]>([]);
    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await http.api.productGetAllPapersWithProperties();
                const productData: ProductDto[] = response.data.map((item: ProductDto) => ({
                    id: item.id || 0,
                    name: item.name || 'Unknown Product',
                    price: item.price || 0.0,
                    properties: item.properties || [],
                }));

                setProducts(productData);
            } catch (error) {
                setError('Error fetching products');
            } finally {
                setLoading(false);
            }
        };

        const fetchProperties = async () => {
            try {
                const response = await http.api.propertyGetAllProperties();  
                const transformProperties = response.data.map((property: PropertyDto) => ({
                    id: property.id as number,
                    name: property.propertyName as string,
                }));
                setProperties(transformProperties);
            } catch (error) {
                setError('Error fetching properties');
            }
        };
        fetchProducts();
        fetchProperties();
    }, []);

    const handleFilter = async () => {
        console.log("Filter button clicked");
        console.log("Selected Property IDs:", selectedProduct);

        try {
            const response = await http.api.productGetPapersByProperties({
                propertyIds: selectedProduct, // This should be an array of selected IDs
            });

            console.log("API Response:", response.data); // Check what you receive from the API

            if (Array.isArray(response.data) && response.data.length > 0) {
                setProducts(response.data);
                console.log("Updated products state: ", response.data);
            } else {
                console.warn("No products found for selected properties.");
            }
        } catch (error) {
            console.error('Error fetching filtered products:', error);
            setError('Error fetching filtered products');
        }
    };




    const handlePropertySelection = (propertyId: number | undefined) => {
        if (propertyId !== undefined) {
            setSelectedProduct(prev =>
                prev.includes(propertyId) ? prev.filter(id => id !== propertyId) : [...prev, propertyId]
            );
        }
    };
    
    if (loading) {
        return <div>Loading...</div>; 
    }

    if (error) {
        return <div>{error}</div>; 
    }

    return (
        <div className="background">
            <h1 className="product-text-header">Product List</h1>
            <div>
                <h2>Filter by Properties</h2>
                {properties.map(property => (
                    <div key={property.id}>
                        <label>
                            <input
                                type="checkbox"
                                checked={selectedProduct.includes(property.id)}
                                onChange={() => handlePropertySelection(property.id)}
                            />
                            {property.name}
                        </label>
                    </div>
                ))}
                <button onClick={handleFilter}>Filter</button>
            </div>
            <ProductList products={products} addToCart={addToCart}/>
        </div>
    );
};

export default Webshop;