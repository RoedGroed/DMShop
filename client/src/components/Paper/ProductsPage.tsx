import React from "react";
import PaperList from "./PaperList.tsx";
import CreatePaperButton from "./CreatePaper.tsx";
import { Toaster } from 'react-hot-toast';

const ProductsPage: React.FC = () => {
    return (
        <div className="min-h-screen bg-customBlue flex flex-col items-center p-4 space-y-8">
            <header className="w-full max-w-5xl flex justify-between items-center">
                <CreatePaperButton />
            </header>
            <section className="w-full max-w-5xl">
                <Toaster />
                <PaperList />
            </section>
        </div>
    );
};

export default ProductsPage;