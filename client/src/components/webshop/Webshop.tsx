import '../../ProductList.css';
import ProductList from './ProductsList.tsx';
import React, { useEffect, useState } from "react";
import { http } from "../../http";
import { ProductDto, PropertyDto } from "../../Api.ts";
import { useCart } from "./CartContext.tsx";

const Webshop: React.FC = () => {
    const [products, setProducts] = useState<ProductDto[]>([]);
    const [filteredProducts, setFilteredProducts] = useState<ProductDto[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const { addToCart } = useCart();
    const [selectedProperties, setSelectedProperties] = useState<number[]>([]);
    const [properties, setProperties] = useState<PropertyDto[]>([]);
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [sortOption, setSortOption] = useState<string>("");

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
                setFilteredProducts(productData); // Initially display all products
            } catch (error) {
                setError('Error fetching products');
            } finally {
                setLoading(false);
            }
        };

        const fetchProperties = async () => {
            try {
                const response = await http.api.propertyGetAllProperties();
                const transformedProperties = response.data.map((property: PropertyDto) => ({
                    id: property.id,
                    propertyName: property.propertyName
                }));
                setProperties(transformedProperties);
            } catch (error) {
                setError('Error fetching properties');
            }
        };

        fetchProducts();
        fetchProperties();
    }, []);

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;
        setSearchTerm(value);
        applyFilters(value, selectedProperties, sortOption);
    };

    const handlePropertySelection = (propertyId: number) => {
        const updatedProperties = selectedProperties.includes(propertyId)
            ? selectedProperties.filter(id => id !== propertyId)
            : [...selectedProperties, propertyId];

        setSelectedProperties(updatedProperties);
        applyFilters(searchTerm, updatedProperties, sortOption);
    };

    const handleSortChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value;
        setSortOption(value);
        applyFilters(searchTerm, selectedProperties, value);
    };

    const applyFilters = (searchTerm: string, selectedProperties: number[], sortOption: string) => {
        let filtered = products.filter(product =>
            product.name.toLowerCase().includes(searchTerm.toLowerCase()) &&
            (selectedProperties.length === 0 ||
                selectedProperties.every(propId => product.properties.some(p => p.id === propId)))
        );

        // Apply sorting based on the selected option
        filtered = filtered.sort((a, b) => {
            if (sortOption === "price") return a.price - b.price;
            if (sortOption === "alphabetical") return a.name.localeCompare(b.name);
            return 0;
        });

        setFilteredProducts(filtered);
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

            <div className="search-and-filter">
                {/* Search Bar */}
                <input
                    type="text"
                    value={searchTerm}
                    onChange={handleSearchChange}
                    placeholder="Search products"
                    className="search-input"
                />

                {/* Sort Dropdown */}
                <select value={sortOption} onChange={handleSortChange} className="sort-dropdown">
                    <option value="">Sort by</option>
                    <option value="price">Price</option>
                    <option value="alphabetical">Alphabetical</option>
                </select>


                {/* Property Filter List */}
                <div className="property-filter">
                    <h2 className="property-filter-header">Filter by Properties</h2>
                    {properties.map(property => (
                        <label key={property.id} className="property-label">
                            <input
                                type="checkbox"
                                checked={selectedProperties.includes(property.id)}
                                onChange={() => handlePropertySelection(property.id)}
                                className="property-checkbox"
                            />
                            <span className="property-name">{property.propertyName || "Unnamed Property"}</span>
                        </label>
                    ))}
                </div>
            </div>

            <ProductList products={filteredProducts} addToCart={addToCart}/>
        </div>
    );
};

export default Webshop;
