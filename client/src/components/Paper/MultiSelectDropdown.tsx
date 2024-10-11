import React, { useState } from "react";
import { PropertyDto } from "../../Api.ts";

interface MultiSelectDropdownProps {
    options: PropertyDto[];
    selectedIds: number[];
    onSelectionChange: (ids: number[]) => void;
}

export const MultiSelectDropdown: React.FC<MultiSelectDropdownProps> = ({ options, selectedIds, onSelectionChange }) => {
    const [isOpen, setIsOpen] = useState(false);
    const toggleDropdown = () => setIsOpen(!isOpen);

    const handleSelection = (id: number) => {
        const newSelection = selectedIds.includes(id)
            ? selectedIds.filter(selectedId => selectedId !== id)
            : [...selectedIds, id];
        onSelectionChange(newSelection);
    };

    return (
        <div className="relative">
            <button onClick={toggleDropdown} className="btn w-full bg-gray-900 rounded-lg hover:bg-blue-600 cursor-pointer">
                {selectedIds.length > 0
                    ? `Selected ${selectedIds.length} properties`
                    : "Select Properties"}
            </button>
            {isOpen && (
                <ul className="absolute bg-customBlue border rounded shadow max-h-48 overflow-y-auto mt-1 w-full grid grid-cols-3 gap-2 p-2">
                    {options.map(option => (
                        <li key={option.id} className="p-1 flex items-center hover:bg-blue-600 cursor-pointer">
                            <input
                                type="checkbox"
                                checked={selectedIds.includes(option.id!)}
                                onChange={() => handleSelection(option.id!)}
                                className="mr-2"
                            />
                            {option.propertyName}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};
