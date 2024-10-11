import {Route, Routes} from "react-router-dom";
import React, {useEffect, useState} from "react";
import NavigationBar from "./NavigationBar.tsx";
import Webshop from "./webshop/Webshop.tsx";
import Contact from "./Other/Contact";
import About from "./Other/About";
import Home from "./Other/Home";
import OrdersList from "./Order/OrdersList";
import ProductsPage from "./Paper/ProductsPage.tsx";
import {CartItem} from "./webshop/Interface.tsx";
import {CartProvider} from "./webshop/CartContext.tsx";
import CustomerOrderHistory from "./Order/CustomerOrderHistory"
import { Toaster } from "react-hot-toast";

const App = () => {


    return (<>
        <CartProvider>
        <NavigationBar />
        <Toaster />
        <Routes>
            <Route path="/" element={<Home />}/>
            <Route path="/webshop" element={<Webshop />} />
            <Route path="/contact" element={<Contact />} />
            <Route path="/about" element={<About />} />
            <Route path="/order" element={<OrdersList />} />
            <Route path="/products" element={<ProductsPage />} />
            <Route path="/myorders" element={<CustomerOrderHistory />} />
        </Routes>
        </CartProvider>

    </>)
}
export default App;


