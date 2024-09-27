import {Route, Routes} from "react-router-dom";
import React, {useEffect} from "react";
import NavigationBar from "./NavigationBar.tsx";
import Webshop from "./Webshop";
import Contact from "./Contact";
import About from "./About";
import Home from "./Home";

const App = () => {


    return (<>
        <NavigationBar />
        <Routes>
            <Route path="/" element={<Home />}/>
            <Route path="/webshop" element={<Webshop />} />
            <Route path="/contact" element={<Contact />} />
            <Route path="/about" element={<About />} />
        </Routes>

    </>)
}
export default App;