import React, { useState } from "react";
import PaperList from "./PaperList.tsx";
import CreatePaperButton from "./CreatePaper.tsx";
import { Toaster } from 'react-hot-toast';
import ManagePropertiesModal from "../Property/ManagePropertiesModal.tsx";

const ProductsPage: React.FC = () => {
    const [isPropertiesModalOpen, setIsPropertiesModalOpen] = useState(false);

    return (
        <div className="min-h-screen bg-customBlue flex flex-col items-center p-4 space-y-8">
            <header className="w-full max-w-5xl flex justify-between items-center space-x-4">
                <div className="flex-shrink-0">
                    <CreatePaperButton />
                </div>
                <h1 className="text-2xl font-bold text-center flex-grow text-white">
                    Product Management
                </h1>
                <div className="flex-shrink-0">
                    <button onClick={() => setIsPropertiesModalOpen(true)} className="btn btn-primary">
                        Manage Properties
                    </button>
                </div>
            </header>
            <section className="w-full max-w-5xl">
                <Toaster />
                <PaperList />
            </section>
            <ManagePropertiesModal
                isOpen={isPropertiesModalOpen}
                onClose={() => setIsPropertiesModalOpen(false)}
            />
        </div>
    );
};

export default ProductsPage;
