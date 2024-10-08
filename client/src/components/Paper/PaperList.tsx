import React, { useEffect, useState } from "react";
import { ProductDto } from "../../Api.ts";
import UpdatePaperModal from "./UpdatePaperModal.tsx";
import { useAtom } from "jotai";
import { PapersAtom } from "./PapersAtom";
import { PropertiesAtom } from "../Property/PropertiesAtom"; // Import the properties atom
import { http } from "../../http.ts";
import PaperItem from "./PaperItem";
import Pagination from "./PaginationProp.tsx";

const ITEMS_PER_PAGE = 10;

const PaperList = () => {
    const [papers, setPapers] = useAtom(PapersAtom);
    const [properties] = useAtom(PropertiesAtom); // Get properties atom
    const [selectedPaper, setSelectedPaper] = useState<ProductDto | null>(null);
    const [isModalOpen, setModalOpen] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);

    // Fetch papers and update atom
    useEffect(() => {
        const fetchPapers = async () => {
            const response = await http.api.productGetAllPapersWithProperties();
            setPapers(response.data);
        };
        fetchPapers();
    }, [setPapers]);

    // Update paper names based on properties
    useEffect(() => {
        setPapers((prevPapers) =>
            prevPapers.map((paper) => ({
                ...paper,
                properties: paper.properties?.map((prop) =>
                    properties.find(p => p.id === prop.id) || prop // Update property name if found
                ),
            }))
        );
    }, [properties, setPapers]);

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

    // Calculate paginated papers
    const startIdx = (currentPage - 1) * ITEMS_PER_PAGE;
    const paginatedPapers = papers.slice(startIdx, startIdx + ITEMS_PER_PAGE);

    return (
        <div className="min-h-screen bg-customBlue flex flex-col items-center justify-center">
            <ul className="space-y-3 w-full max-w-5xl">
                {paginatedPapers.map((paper) => (
                    <PaperItem
                        key={paper.id}
                        paper={paper}
                        openModal={openModal}
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

            {/* Pagination Component */}
            <Pagination
                currentPage={currentPage}
                totalItems={papers.length}
                itemsPerPage={ITEMS_PER_PAGE}
                onPageChange={setCurrentPage}
            />
        </div>
    );
};

export default PaperList;
