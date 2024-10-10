import {Route, Routes} from "react-router-dom";
import React, {useEffect} from "react";
import NavigationBar from "./NavigationBar.tsx";
import Webshop from "./Webshop";
import Contact from "./Contact";
import About from "./About";
import Home from "./Home";
import OrdersList from "./Order/OrdersList";
import ProductsPage from "./Paper/ProductsPage.tsx";
import CustomerOrderHistory from "./Order/CustomerOrderHistory"

const App = () => {


    return (<>
        <NavigationBar />
        <Routes>
            <Route path="/" element={<Home />}/>
            <Route path="/webshop" element={<Webshop />} />
            <Route path="/contact" element={<Contact />} />
            <Route path="/about" element={<About />} />
            <Route path="/order" element={<OrdersList />} />
            <Route path="/products" element={<ProductsPage />} />
            <Route path="/myorders" element={<CustomerOrderHistory />} />
        </Routes>

    </>)
}
export default App;


