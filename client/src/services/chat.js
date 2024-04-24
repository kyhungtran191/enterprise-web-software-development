import { allOnlineFacultyUsers, conservationUsers, messageAPI } from "@/apis";
import instanceAxios from "@/utils/axiosInstance";

export const getCurrentOnlineUser = async () => await instanceAxios.get(allOnlineFacultyUsers)

export const createNewConservation = async (specificReceiverId) => await instanceAxios.post(`${conservationUsers}?specificReceiverId=${specificReceiverId}`)

export const getPrivateConservations = async () => await instanceAxios.get(conservationUsers)

export const getDetailConservations = async (specificReceiverId) => await instanceAxios.get(`${conservationUsers}?specificReceiverId=${specificReceiverId}`)

export const addNewMessage = async (body) => await instanceAxios.post(messageAPI, body)