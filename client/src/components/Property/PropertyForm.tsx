import React, { useState, useEffect } from "react";
import { PropertyDto } from "../../Api";

interface PropertyFormProps {
    property: PropertyDto | null;
    onSave: (property: PropertyDto) => void;
}

const PropertyForm: React.FC<PropertyFormProps> = ({ property, onSave }) => {
    const [propertyName, setPropertyName] = useState(property?.propertyName || "");

    useEffect(() => {
        if (property) setPropertyName(property.propertyName || "");
    }, [property]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSave({ id: property?.id, propertyName });
        setPropertyName("");
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-2">
            <input
                type="text"
                value={propertyName}
                onChange={(e) => setPropertyName(e.target.value)}
                placeholder="Property Name"
                className="input text-white input-bordered w-full bg-gray-900"
                required
            />
            <button type="submit" className="btn btn-primary w-full">
                {property ? "Update" : "Add"} Property
            </button>
        </form>
    );
};

export default PropertyForm;
