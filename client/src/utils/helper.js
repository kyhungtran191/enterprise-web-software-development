export function formatDate(inputDate) {
  const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

  const date = new Date(inputDate);

  const day = date.getDate();
  const monthIndex = date.getMonth();
  const year = date.getFullYear();

  const monthName = months[monthIndex];

  const result = `${day} ${monthName} ${year}`;

  return result;
}

export async function convertFilesToBlob(filesArray) {
  const filesPromises = filesArray.map(async fileObj => {
    try {
      const response = await fetch(fileObj.path);
      const blob = await response.blob();
      return new File([blob], fileObj.name, { type: blob.type });
    } catch (error) {
      console.error(`Failed to create File for ${fileObj.name}:`, error);
      return null;
    }
  });

  return Promise.all(filesPromises);
}


export const convertPermissionsToObject = (permissions) => {
  const result = {};

  permissions.forEach(permission => {
    const [resource, action] = permission.split('.').reverse();
    if (!result[resource]) {
      result[resource] = [];
    }
    result[resource].push(action);
  });

  return result;
};

const updateAbilityFromPermissions = (ability, permissions) => {
  permissions.forEach(permission => {
    const { action, subject } = permission;
    switch (action.toUpperCase()) {
      case 'CREATE':
        ability.update([{ action: 'create', subject }]);
        break;
      case 'READ':
        ability.update([{ action: 'read', subject }]);
        break;
      case 'UPDATE':
        ability.update([{ action: 'update', subject }]);
        break;
      case 'DELETE':
        ability.update([{ action: 'delete', subject }]);
        break;
      case 'APPROVE':
        ability.update([{ action: 'approve', subject }]);
        break;
      default:
        console.error('Invalid action:', action);
        break;
    }
  });
};
