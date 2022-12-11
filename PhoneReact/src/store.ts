import jotai from 'jotai';


export type Computer = {
  id: number;
  hostname: string;
  ip: string;
  username: string;
  password: string;
  isAnonymous: boolean;
}



export const computersAtom = jotai.atom<{
  computers: Computer[];
}>({
  computers: [],
});

