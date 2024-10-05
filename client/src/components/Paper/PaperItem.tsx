import React from 'react';
import { ProductDto } from '../../Api.ts';
import { CogIcon } from '@heroicons/react/24/solid';
import DeletePaperButton from "./DeletePaperButton";

interface PaperItemProps {
    paper: ProductDto;
    openModal: (paper: ProductDto) => void; // Function prop to open modal
}

const PaperItem: React.FC<PaperItemProps> = ({ paper, openModal }) => {
    return (
        <li key={paper.id} className="p-1 bg-gray-900 rounded-lg flex justify-between items-center text-white">
            <p className="flex-1">{paper.name}</p>
            <p className="flex-1">Price: ${paper.price?.toFixed(2) || "0.00"}</p>

            <div className="flex-1 text-xs space-y-0.5">
                <p>Stock: {paper.stock || 0}</p>
                <p>Discontinued: {paper.discontinued ? "Yes" : "No"}</p>
            </div>

            <div className="flex-1">
                <ul>
                    {paper.properties?.map(prop => (
                        <li key={prop.id}>{prop.propertyName}</li>
                    ))}
                </ul>
            </div>

            <div className="flex items-center space-x-2">
                {/* Update Button */}
                <button
                    className="flex items-center justify-center h-6 w-6 rounded-full border border-gray-300 p-0"
                    onClick={() => openModal(paper)}
                >
                    <CogIcon className="w-5 h-5 text-white hover:text-customBlue" />
                </button>

                {/* Pass the entire ProductDto to the DeletePaperButton */}
                <DeletePaperButton product={paper} />
            </div>
        </li>
    );
};

export default PaperItem;
