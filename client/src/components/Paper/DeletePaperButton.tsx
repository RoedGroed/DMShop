import React from "react";
import { toast } from "react-hot-toast";
import { http } from "../../http";
import { useAtom } from "jotai";
import { PapersAtom } from "./PapersAtom";
import { ProductDto } from "../../Api.ts";

interface DeletePaperButtonProps {
    product: ProductDto;
}

const DeletePaperButton: React.FC<DeletePaperButtonProps> = ({ product }) => {
    const [, setPapers] = useAtom(PapersAtom);

    const handleDelete = async () => {
        // Confirmation using the toast
        toast(
            (t) => (
                <div>
                    <p>Are you sure you want to delete this paper?</p>
                    <div className="mt-4 flex justify-end space-x-2">
                        <button
                            onClick={() => {
                                toast.dismiss(t.id); // Dismiss toaster
                                deletePaper(); // Proceed with deletion
                            }}
                            className="btn btn-error"
                        >
                            Yes, Delete
                        </button>
                        <button
                            onClick={() => toast.dismiss(t.id)} // Cancel deletion
                            className="btn btn-outline"
                        >
                            Cancel
                        </button>
                    </div>
                </div>
            ),
            { duration: 5000 } // Show the toast for 5 seconds
        );
    };

    const deletePaper = async () => {
        try {
            // Call the API to delete the paper using the ProductDto
            await http.api.productDeletePaper(product.id!, product); // Pass the ProductDto

            // Update the PapersAtom by removing the deleted paper
            setPapers((prev) => prev.filter((paper) => paper.id !== product.id));

            // Show a success message
            toast.success("Paper deleted successfully!");
        } catch (error) {
            toast.error("Error deleting paper.");
            console.error("Error deleting paper:", error);
        }
    };

    return (
        <button
            className="flex items-center justify-center h-6 w-6 rounded-full border border-red-700 p-0"
            onClick={handleDelete}
            title="Delete Paper"
        >
            <span className="text-red-600 hover:text-red-800">X</span>
        </button>
    );
};

export default DeletePaperButton;
