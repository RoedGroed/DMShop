import React, { useState } from "react";
import { ProductDto } from "../../Api.ts";
import UpdatePaperModal from "./UpdatePaperModal.tsx";
import { http } from "../../http.ts";
import { useAtom } from "jotai";
import { PapersAtom } from "../../atoms/PapersAtom.ts";
import toast from "react-hot-toast";

const CreatePaper: React.FC = () => {
    const [isModalOpen, setModalOpen] = useState(false);
    const [papers, setPapers] = useAtom(PapersAtom);

    const emptyPaper: ProductDto = {
        id: undefined,
        name: '',
        price: 0,
        stock: 0,
        discontinued: false,
        properties: []
    };

    const openModal = () => {
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
    };


    const createPaper = async (newPaper: ProductDto) => {
        try {
            // Call the API to create the new paper
            const response = await http.api.productCreatePaper(newPaper);
            // Update the atom with the newly created paper
            toast.success("Paper Created")
            setPapers((prev) => [...prev, response.data]);
        } catch (error) {
            toast.error("Error creating paper: " + error);
        }
        closeModal();
    };

    return (
        <>
            <button className="btn btn-primary" onClick={openModal}>
                Create New Paper
            </button>

            {isModalOpen && (
                <UpdatePaperModal
                    paper={emptyPaper}  // Pass empty paper to modal for creation
                    onUpdate={createPaper}  // Pass the createPaper function to the modal
                    onClose={closeModal}
                />
            )}
        </>
    );
};

export default CreatePaper;
