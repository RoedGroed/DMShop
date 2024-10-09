import App from "./App.tsx";
import '../ProductList.css';
import ProductList from './webshop/ProductsList.tsx'
import {useEffect, useState} from "react";
import { http } from "../http";
import {ProductDto} from "../Api.ts";
import NavigationBar from "./NavigationBar.tsx";
import axios from "axios";

interface Product {
    id: number;
    name?: string;
}

const Webshop: React.FC = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [cart, setCart] = useState<Product[]>([]);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await http.api.productGetAllPapers();

                const productData: Product[] = response.data.map((item: ProductDto) => ({
                    id: item.id || 0,
                    name: item.name || 'Unknown Product',
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

    const addToCart = (product: Product) => {
        setCart((prevCart) => [...prevCart, product]);
        console.log(product);
    };

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