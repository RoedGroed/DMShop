import React, { useEffect } from "react";
import { ProductDto } from "../../Api.ts";
import UpdatePaperModal from "./UpdatePaperModal.tsx";
import { useAtom } from "jotai";
import { PapersAtom } from "./PapersAtom";
import { http } from "../../http.ts";
import PaperItem from "./PaperItem";

const PaperList = () => {
    const [papers, setPapers] = useAtom(PapersAtom);
    const [selectedPaper, setSelectedPaper] = React.useState<ProductDto | null>(null);
    const [isModalOpen, setModalOpen] = React.useState(false);

    // Fetch papers and update atom
    useEffect(() => {
        const fetchPapers = async () => {
            const response = await http.api.productGetAllPapersWithProperties();
            setPapers(response.data);
        };
        fetchPapers();
    }, [setPapers]);

    const openModal = (paper: ProductDto) => {
        setSelectedPaper(paper);
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelectedPaper(null);
    };

    const updatePaper = async (updatedPaper: ProductDto) => {
        await http.api.productUpdatePaper(updatedPaper.id!, updatedPaper);

        // Update the local atom state
        setPapers((prev) =>
            prev.map((paper) =>
                paper.id === updatedPaper.id ? updatedPaper : paper
            )
        );
        closeModal();
    };

    return (
        <div className="min-h-screen bg-customBlue flex flex-col items-center justify-center">
            <ul className="space-y-3 w-full max-w-5xl">
                {papers.map((paper) => (
                    <PaperItem
                        key={paper.id}  // Key should still be provided here
                        paper={paper}
                        openModal={openModal}  // Pass the openModal function to PaperItem
                    />
                ))}
            </ul>

            {isModalOpen && selectedPaper && (
                <UpdatePaperModal
                    paper={selectedPaper}
                    onUpdate={updatePaper}
                    onClose={closeModal}
                />
            )}
        </div>
    );
};

export default PaperList;
