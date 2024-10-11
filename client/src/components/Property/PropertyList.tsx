import React, { useEffect } from "react";
import PropertyItem from "./PropertyItem";
import { useAtom } from "jotai";
import { PropertiesAtom } from "../../atoms/PropertiesAtom.ts";
import { http } from "../../http.ts";

const PropertyList: React.FC<{ onEdit: (id: number) => void; onDelete: (id: number) => void }> = ({ onEdit, onDelete }) => {
    const [properties, setProperties] = useAtom(PropertiesAtom);

    useEffect(() => {
        const fetchProperties = async () => {
            const response = await http.api.propertyGetAllProperties();
            setProperties(response.data);
        };
        fetchProperties();
    }, [setProperties]);

    return (
        <div className="bg-customBlue rounded shadow max-h-80 overflow-y-auto">
            <ul className="space-y-3 w-full">
                {properties.map((property) => (
                    <PropertyItem
                        key={property.id}
                        property={property}
                        onEdit={onEdit} // Pass the onEdit prop
                        onDelete={onDelete} // Pass the onDelete prop
                    />
                ))}
            </ul>
        </div>
    );
};

export default PropertyList;
