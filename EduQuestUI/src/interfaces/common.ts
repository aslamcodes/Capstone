export interface Tab {
  label: string;
  value: string;
}

export interface UserProfile {
  firstName: string;
  lastName: string;
  isEducator: boolean;
  email: string;
  profilePictureUrl: string | null;
}
