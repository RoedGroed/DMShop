import React, { useState, useEffect } from "react";
import { ProductDto, PropertyDto } from "../../Api.ts";
import { http } from "../../http.ts";
import { MultiSelectDropdown } from "./MultiSelectDropdown";

interface UpdatePaperModalProps {
    paper: ProductDto;
    onUpdate: (updatedPaper: ProductDto) => void;
    onClose: () => void;
}

const UpdatePaperModal: React.FC<UpdatePaperModalProps> = ({ paper, onUpdate, onClose }) => {
    const [name, setName] = useState(paper?.name || "");
    const [price, setPrice] = useState(paper?.price || 0);
    const [stock, setStock] = useState(paper?.stock || 0);
    const [discontinued, setDiscontinued] = useState(paper?.discontinued || false);
    const [selectedPropertyIds, setSelectedPropertyIds] = useState<number[]>(paper?.properties?.map(p => p.id!) || []);
    const [availableProperties, setAvailableProperties] = useState<PropertyDto[]>([]);

    useEffect(() => {
        const fetchProperties = async () => {
            try {
                const response = await http.api.propertyGetAllProperties();
                setAvailableProperties(response.data);
            } catch (error) {
                console.error("Error fetching properties:", error);
            }
        };
        fetchProperties();
    }, []);

    const handleUpdate = () => {
        const updatedPaper: ProductDto = {
            ...paper,
            name,
            price,
            stock,
            discontinued,
            properties: selectedPropertyIds.map(id => {
                const property = availableProperties.find(prop => prop.id === id);
                return {
                    id: property?.id,
                    propertyName: property?.propertyName || "",
                } as PropertyDto;
            }),
        };
        onUpdate(updatedPaper);
    };

    return (
        <div className="fixed inset-0 flex items-center justify-center z-50">
            <div className="fixed inset-0 bg-black opacity-30" onClick={onClose}></div>
            <div className="bg-customBlue text-white rounded-lg p-6 w-1/4 relative">
                <h3 className="font-bold text-lg mb-4 text-white">
                    {paper?.id ? `Editing Paper: ${paper?.name}` : "Creating New Paper"}
                </h3>

                <label className="text-white mb-1 block">Name</label>
                <input
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder="Enter Paper Name"
                    className="input w-full mb-3 bg-gray-900"
                />

                <label className="text-white mb-1 block ">Price</label>
                <input
                    type="number"
                    value={price}
                    onChange={(e) => setPrice(Number(e.target.value))}
                    placeholder="Enter Price"
                    className="input w-full mb-3 bg-gray-900"
                />

                <label className="text-white mb-1 block">Stock</label>
                <input
                    type="number"
                    value={stock}
                    onChange={(e) => setStock(Number(e.target.value))}
                    placeholder="Enter Stock Quantity"
                    className="input w-full mb-3 bg-gray-900"
                />

                <label className="text-white mb-1 flex items-center">
                    <input
                        type="checkbox"
                        checked={discontinued}
                        onChange={() => setDiscontinued(!discontinued)}
                        className="mr-2 bg-gray-900"
                    />
                    Discontinued
                </label>

                <div className="mt-4 ">
                    <MultiSelectDropdown
                        options={availableProperties}
                        selectedIds={selectedPropertyIds}
                        onSelectionChange={setSelectedPropertyIds}
                    />
                </div>

                <div className="flex justify-end mt-4">
                    <button className="btn btn-outline mr-2 bg-red-700" onClick={onClose}>Cancel</button>
                    <button className="btn btn-primary" onClick={handleUpdate}>
                        {paper?.id ? "Update" : "Create"}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default UpdatePaperModal;
