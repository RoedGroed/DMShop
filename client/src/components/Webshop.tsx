import '../ProductList.css';
import ProductList from './webshop/ProductsList.tsx'
import React, {useEffect, useState} from "react";
import { http } from "../http";
import {ProductDto} from "../Api.ts";
import {useCart} from "./webshop/CartContext.tsx";



const Webshop: React.FC = () => {
    const [products, setProducts] = useState<ProductDto[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const {addToCart} = useCart();
    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await http.api.productGetAllPapersWithProperties();
                setProducts(products);
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

        fetchProducts();
    }, []);


    if (loading) {
        return <div>Loading...</div>; // Loading state
    }

    if (error) {
        return <div>{error}</div>; // Error state
    }

    return (
        <div className="background">
            <h1 className="product-text-header">Product List</h1>
            <ProductList products={products} addToCart={addToCart} />
        </div>
    );
};

export default Webshop;